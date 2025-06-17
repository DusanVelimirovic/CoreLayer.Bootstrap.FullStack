using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    public interface IAdminUserService
    {
        Task<PaginatedResult<UserListDto>> GetUsersAsync(int page, int pageSize, string? search = null);
        Task<bool> UpdateUserAsync(UpdateUserDto dto);
        Task<(bool Success, string? ErrorMessage)> CreateUserAsync(CreateUserDto dto);
        Task<bool> AssignRoleToUserAsync(string userId, string roleId);
    }
}
