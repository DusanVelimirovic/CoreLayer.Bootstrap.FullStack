using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    /// <summary>
    /// Provides administrative services for managing access control to application modules by role.
    /// </summary>
    public interface IAdminAccessControlService
    {
        /// <summary>
        /// Retrieves all module access control entries, indicating which roles can access which modules.
        /// </summary>
        /// <returns>
        /// A list of <see cref="AccessControlDto"/> entries representing the access permissions.
        /// </returns>
        Task<List<AccessControlDto>> GetAccessControlsAsync();

        /// <summary>
        /// Retrieves all module access control entries, indicating which roles can access which modules.
        /// </summary>
        /// <returns>
        /// A list of <see cref="AccessControlDto"/> entries representing the access permissions.
        /// </returns>
        Task<bool> UpdateAccessControlAsync(AccessControlDto dto);
    }
}
