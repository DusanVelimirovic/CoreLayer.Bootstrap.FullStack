using WebApp.DTOs.Identity;
using WebApp.DTOs.Common;

namespace WebApp.Services.Identity.Admin
{
    /// <summary>
    /// Provides administrative operations for managing users, including creation, updates, and role assignments.
    /// </summary>
    public class AdminUserService
    {
        private readonly HttpClient _httpClient;

        public AdminUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a paginated list of users, optionally filtered by a search query.
        /// </summary>
        /// Preconditions:
        /// - Admin must be authenticated and authorized to access user data.
        /// - `page` and `pageSize` must be positive integers.
        /// - `search` is optional and will be URI-escaped.
        ///
        /// Postconditions:
        /// - Returns a paginated result of users or an empty result if the API returns null.
        public async Task<PaginatedResult<UserListDto>> GetUsersAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            var url = $"/api/admin/users?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";

            var result = await _httpClient.GetFromJsonAsync<PaginatedResult<UserListDto>>(url);
            return result ?? new PaginatedResult<UserListDto>();
        }

        /// <summary>
        /// Creates a new user based on the provided DTO.
        /// </summary>
        /// Preconditions:
        /// - `dto` must be a valid user creation object with all required fields.
        ///
        /// Postconditions:
        /// - Sends data to the backend and returns success flag and optional message.
        public async Task<(bool Success, string? Message)> CreateUserAsync(CreateUserDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/users", dto);
            var result = await response.Content.ReadFromJsonAsync<GenericApiResponse>();

            return (response.IsSuccessStatusCode, result?.Message);
        }

        /// <summary>
        /// Updates an existing user with new information.
        /// </summary>
        /// Preconditions:
        /// - `dto` must contain a valid `UserId` and updated fields.
        ///
        /// Postconditions:
        /// - Sends PUT request to the API; returns true if successful.
        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/admin/users/{dto.UserId}", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Assigns a specific role to a user.
        /// </summary>
        /// Preconditions:
        /// - `userId` and `roleId` must be valid, non-empty strings.
        ///
        /// Postconditions:
        /// - Sends role assignment payload to the API; returns true if successful.
        public async Task<bool> AssignRoleAsync(string userId, string roleId)
        {
            var payload = new AssignUserRoleDto
            {
                UserId = userId,
                RoleId = roleId
            };

            var response = await _httpClient.PostAsJsonAsync($"/api/admin/users/{userId}/assign-role", payload);
            return response.IsSuccessStatusCode;
        }
    }

    /// <summary>
    /// DTO for assigning a role to a user.
    /// </summary>
    public class AssignUserRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Generic API response wrapper containing success status and an optional message.
    /// </summary>
    public class GenericApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
