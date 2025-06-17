using Microsoft.AspNetCore.Identity;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    public class AdminRoleService : IAdminRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminRoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = _roleManager.Roles.ToList();
            var result = roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name ?? ""
            }).ToList();

            return Task.FromResult(result);
        }

        public async Task<bool> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = new ApplicationRole
            {
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> UpdateRoleAsync(string roleId, UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            role.Name = dto.Name;
            role.NormalizedName = dto.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
