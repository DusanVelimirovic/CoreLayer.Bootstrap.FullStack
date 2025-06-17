using Microsoft.AspNetCore.Identity;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    /// <summary>
    /// Service responsible for managing application roles.
    /// </summary>
    /// <remarks>
    /// Provides administrative capabilities to list, create, and update roles.
    /// </remarks>
    public class AdminRoleService : IAdminRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRoleService"/> class.
        /// </summary>
        /// <param name="roleManager">ASP.NET Core Identity role manager for <see cref="ApplicationRole"/>.</param>
        public AdminRoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Retrieves all roles from the system.
        /// </summary>
        /// <returns>
        /// A task that returns a list of <see cref="RoleDto"/> representing each application role.
        /// </returns>
        /// <remarks>
        /// Useful for populating role dropdowns or access control configuration screens.
        /// </remarks>
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

        /// <summary>
        /// Creates a new role in the system.
        /// </summary>
        /// <param name="dto">DTO containing the role name to be created.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is <c>true</c> if the role was successfully created; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Role name is automatically normalized to uppercase for internal consistency.
        /// </remarks>
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

        /// <summary>
        /// Updates the name of an existing role.
        /// </summary>
        /// <param name="roleId">The ID of the role to update.</param>
        /// <param name="dto">DTO containing the new role name.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is <c>true</c> if the update was successful; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The role's normalized name is also updated to match the new name.
        /// </remarks>
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
