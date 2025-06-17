using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Services.Identity
{
    public class AdminAccessControlService : IAdminAccessControlService
    {
        private readonly CoreLayerDbContext _context;

        public AdminAccessControlService(CoreLayerDbContext context)
        {
            _context = context;
        }

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
