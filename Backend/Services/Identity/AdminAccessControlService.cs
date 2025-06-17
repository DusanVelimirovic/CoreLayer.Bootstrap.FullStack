using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    /// <summary>
    /// Service responsible for managing module access control by roles.
    /// </summary>
    /// <remarks>
    /// Handles retrieval and updates of access permissions between roles and modules.
    /// </remarks>
    public class AdminAccessControlService : IAdminAccessControlService
    {
        private readonly CoreLayerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminAccessControlService"/> class.
        /// </summary>
        /// <param name="context">The database context for access control data operations.</param>
        public AdminAccessControlService(CoreLayerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all role-to-module access control records.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a list of <see cref="AccessControlDto"/> items.
        /// </returns>
        /// <remarks>
        /// Useful for administrative views of access rights per module and role.
        /// </remarks>
        public async Task<List<AccessControlDto>> GetAccessControlsAsync()
        {
            var accessControls = await _context.ModuleAccessControls
                .Select(ac => new AccessControlDto
                {
                    ModuleId = ac.ModuleId,
                    RoleId = ac.RoleId,
                    CanAccess = ac.CanAccess
                }).ToListAsync();

            return accessControls;
        }

        /// <summary>
        /// Creates or updates access control for a specific role-module combination.
        /// </summary>
        /// <param name="dto">The data transfer object containing module ID, role ID, and access flag.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result is <c>true</c> if the update or insert was successful.
        /// </returns>
        /// <remarks>
        /// If a matching record exists, the <c>CanAccess</c> flag is updated. Otherwise, a new record is added.
        /// </remarks>
        public async Task<bool> UpdateAccessControlAsync(AccessControlDto dto)
        {
            var existing = await _context.ModuleAccessControls
                .FirstOrDefaultAsync(ac => ac.ModuleId == dto.ModuleId && ac.RoleId == dto.RoleId);

            if (existing != null)
            {
                existing.CanAccess = dto.CanAccess;
            }
            else
            {
                _context.ModuleAccessControls.Add(new ModuleAccessControl
                {
                    ModuleId = dto.ModuleId,
                    RoleId = dto.RoleId,
                    CanAccess = dto.CanAccess
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
