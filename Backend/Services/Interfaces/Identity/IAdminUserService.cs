using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    /// <summary>
    /// Provides administrative operations for managing application users, including retrieval, update, creation, and role assignment.
    /// </summary>
    public interface IAdminUserService
    {
        /// <summary>
        /// Retrieves a paginated list of users, optionally filtered by a search term.
        /// </summary>
        /// <param name="page">The current page number (1-based).</param>
        /// <param name="pageSize">The number of users to return per page.</param>
        /// <param name="search">An optional search string to filter users by email or username.</param>
        /// <returns>
        /// A <see cref="PaginatedResult{UserListDto}"/> containing the filtered and paginated list of users.
        /// </returns>
        Task<PaginatedResult<UserListDto>> GetUsersAsync(int page, int pageSize, string? search = null);

        /// <summary>
        /// Updates user profile data including email, username, role, activation status, and 2FA settings.
        /// </summary>
        /// <param name="dto">The updated user information.</param>
        /// <returns>
        /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> UpdateUserAsync(UpdateUserDto dto);

        /// <summary>
        /// Creates a new user account and optionally assigns the user to a role.
        /// </summary>
        /// <param name="dto">The data needed to create a user, including username, email, password, and role ID.</param>
        /// <returns>
        /// A tuple containing a success flag and an optional error message if the operation fails.
        /// </returns>
        Task<(bool Success, string? ErrorMessage)> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Assigns a specific role to an existing user, replacing any previously assigned roles.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role should be assigned.</param>
        /// <param name="roleId">The ID of the role to assign.</param>
        /// <returns>
        /// <c>true</c> if the role assignment was successful; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> AssignRoleToUserAsync(string userId, string roleId);
    }
}
