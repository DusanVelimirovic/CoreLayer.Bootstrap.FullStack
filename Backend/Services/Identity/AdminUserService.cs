using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    /// <summary>
    /// Service responsible for user administration including retrieval, creation, updates, and role assignments.
    /// </summary>
    /// <remarks>
    /// Used by administrators to manage users within the application.
    /// </remarks>
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CoreLayerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserService"/> class.
        /// </summary>
        /// <param name="userManager">ASP.NET Identity user manager for managing application users.</param>
        /// <param name="context">The application's database context.</param>
        public AdminUserService(UserManager<ApplicationUser> userManager, CoreLayerDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of users, optionally filtered by a search term.
        /// </summary>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Number of users per page.</param>
        /// <param name="search">Optional search string to match against email or username.</param>
        /// <returns>
        /// A paginated result containing a list of <see cref="UserListDto"/> items with user and role details.
        /// </returns>
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

        /// <summary>
        /// Updates user account data such as email, username, 2FA status, activity status, and assigned role.
        /// </summary>
        /// <param name="dto">DTO containing user ID and updated fields.</param>
        /// <returns><c>true</c> if update succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// If a new role ID is provided, all existing roles will be removed and replaced with the specified role.
        /// </remarks>
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

        /// <summary>
        /// Creates a new user account and assigns the specified role.
        /// </summary>
        /// <param name="dto">DTO containing the user's registration information.</param>
        /// <returns>
        /// A tuple where <c>Success</c> indicates whether creation succeeded, and <c>ErrorMessage</c> contains details if it failed.
        /// </returns>
        /// <remarks>
        /// The user's email is automatically marked as confirmed and 2FA is disabled by default.
        /// </remarks>
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

        /// <summary>
        /// Assigns a single role to the user by first removing all currently assigned roles.
        /// </summary>
        /// <param name="userId">The ID of the user to modify.</param>
        /// <param name="roleId">The ID of the new role to assign.</param>
        /// <returns><c>true</c> if role assignment succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// Useful for admin UI where roles are selected from a dropdown and should replace any previous assignment.
        /// </remarks>
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
