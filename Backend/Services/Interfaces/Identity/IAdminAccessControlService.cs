using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    public interface IAdminAccessControlService
    {
        Task<List<AccessControlDto>> GetAccessControlsAsync();
        Task<bool> UpdateAccessControlAsync(AccessControlDto dto);
    }
}
