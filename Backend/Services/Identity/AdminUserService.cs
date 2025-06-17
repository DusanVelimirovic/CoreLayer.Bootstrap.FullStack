using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CoreLayerDbContext _context;

        public AdminUserService(UserManager<ApplicationUser> userManager, CoreLayerDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Gte Users
        public async Task<PaginatedResult<UserListDto>> GetUsersAsync(int page, int pageSize, string? search = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(u =>
                    u.Email.ToLower().Contains(search) ||
                    u.UserName.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PaginatedResult<UserListDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Items.Add(new UserListDto
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    UserName = user.UserName ?? "",
                    IsActive = user.IsActive,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        // Update Users
        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                user.UserName = dto.UserName;

            if (dto.TwoFactorEnabled.HasValue)
                user.TwoFactorEnabled = dto.TwoFactorEnabled.Value;

            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

            // Update role if specified
            if (!string.IsNullOrEmpty(dto.RoleId))
            {
                var existingRoles = await _userManager.GetRolesAsync(user);
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
                if (role == null) return false;

                await _userManager.RemoveFromRolesAsync(user, existingRoles);
                await _userManager.AddToRoleAsync(user, role.Name!);
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // Create user
        public async Task<(bool Success, string? ErrorMessage)> CreateUserAsync(CreateUserDto dto)
        {
            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
                return (false, "Rola nije pronadjena.");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = true,
                IsActive = true,
                TwoFactorEnabled = false,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            var roleAssignResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!roleAssignResult.Succeeded)
            {
                var errors = string.Join("; ", roleAssignResult.Errors.Select(e => e.Description));
                return (false, $"Korisnik je kreiran ali pokusaj dodeljivanja role nije uspeo: {errors}");
            }

            return (true, null);
        }

        // Assign Role to user
        public async Task<bool> AssignRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            // Remove current roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded) return false;

            // Add new role
            var addResult = await _userManager.AddToRoleAsync(user, role.Name);
            return addResult.Succeeded;
        }
    }
}
