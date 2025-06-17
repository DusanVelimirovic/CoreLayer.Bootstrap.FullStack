using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    /// <summary>
    /// Provides administrative services for managing application roles.
    /// </summary>
    public interface IAdminRoleService
    {
        /// <summary>
        /// Retrieves a list of all application roles.
        /// </summary>
        /// <returns>
        /// A list of <see cref="RoleDto"/> objects representing the roles in the system.
        /// </returns>
        Task<List<RoleDto>> GetRolesAsync();

        /// <summary>
        /// Creates a new application role.
        /// </summary>
        /// <param name="dto">The role data required to create a new role.</param>
        /// <returns>
        /// <c>true</c> if the role was successfully created; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> CreateRoleAsync(CreateRoleDto dto);

        /// <summary>
        /// Updates the details of an existing role by its identifier.
        /// </summary>
        /// <param name="roleId">The ID of the role to update.</param>
        /// <param name="dto">The updated role data.</param>
        /// <returns>
        /// <c>true</c> if the role was successfully updated; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> UpdateRoleAsync(string roleId, UpdateRoleDto dto);
    }
}
