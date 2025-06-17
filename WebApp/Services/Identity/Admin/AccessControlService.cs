using WebApp.DTOs.Identity;

namespace WebApp.Services.Identity.Admin
{
    public class AccessControlService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Provides methods to retrieve and update access control settings for role-module relationships.
        /// </summary>
        public AccessControlService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves the current access control configuration for all roles and modules.
        /// </summary>
        /// Preconditions:
        /// - The user must be authenticated and authorized to access `/api/admin/access-control`.
        ///
        /// Postconditions:
        /// - Returns a list of <see cref="AccessControlDto"/> representing role-module access pairs.
        /// - If the response is null, returns an empty list instead of throwing an exception.
        public async Task<List<AccessControlDto>> GetAccessControlsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<AccessControlDto>>("/api/admin/access-control");
            return response ?? new List<AccessControlDto>();
        }

        /// <summary>
        /// Updates or creates a role-module access control entry on the backend.
        /// </summary>
        /// Preconditions:
        /// - `dto` must be a valid access control object with existing RoleId and ModuleId.
        /// - The user must be authorized to update access settings.
        ///
        /// Postconditions:
        /// - Sends the DTO to the API and returns true if the operation was successful (HTTP 2xx).
        /// - Returns false on failure (e.g. 400 or 500 errors).
        public async Task<bool> UpdateAccessControlAsync(AccessControlDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/access-control", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
