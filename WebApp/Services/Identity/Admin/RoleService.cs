using WebApp.DTOs.Identity;

namespace WebApp.Services.Identity.Admin
{
    /// <summary>
    /// Provides admin functionality for managing user roles, including retrieval, creation, and updates.
    /// </summary>
    public class RoleService
    {
        private readonly HttpClient _httpClient;

        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves all roles from the backend.
        /// </summary>
        /// Preconditions:
        /// - Admin must be authenticated and authorized to read roles.
        ///
        /// Postconditions:
        /// - Returns a list of roles or an empty list if the response is null.
        public async Task<List<RoleDto>> GetRolesAsync() 
        {
            var response = await _httpClient.GetFromJsonAsync<List<RoleDto>>("/api/admin/roles");
            return response ?? new List<RoleDto>();
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// Preconditions:
        /// - `dto` must contain a valid role name and other required metadata.
        ///
        /// Postconditions:
        /// - Sends a POST request to create a new role and returns success status.
        public async Task<bool> CreateRoleAsync(CreateRoleDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/roles", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates an existing role by its ID.
        /// </summary>
        /// Preconditions:
        /// - `id` must refer to an existing role.
        /// - `dto` must contain updated role data.
        ///
        /// Postconditions:
        /// - Sends a PUT request to update the role and returns true if successful.
        public async Task<bool> UpdateRoleAsync(string id, UpdateRoleDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/admin/roles/{id}", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
