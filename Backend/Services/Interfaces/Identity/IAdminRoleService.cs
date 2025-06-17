using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    public interface IAdminRoleService
    {
        Task<List<RoleDto>> GetRolesAsync();
        Task<bool> CreateRoleAsync(CreateRoleDto dto);
        Task<bool> UpdateRoleAsync(string roleId, UpdateRoleDto dto);
    }
}
