using WebApp.DTOs.Identity;

namespace WebApp.Services.Identity.Admin
{
    /// <summary>
    /// Provides admin functionality for managing application modules, including retrieval, creation, and updates.
    /// </summary>
    public class ModuleService
    {
        private readonly HttpClient _httpClient;

        public ModuleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves the list of all modules registered in the system.
        /// </summary>
        /// Preconditions:
        /// - Admin must be authenticated and authorized to access modules.
        ///
        /// Postconditions:
        /// - Returns a list of modules or an empty list if API returns null.
        public async Task<List<ModuleDto>> GetModulesAsync()
        {
            var modules = await _httpClient.GetFromJsonAsync<List<ModuleDto>>("/api/admin/modules");
            return modules ?? new List<ModuleDto>();
        }

        /// <summary>
        /// Creates a new module entry in the system.
        /// </summary>
        /// Preconditions:
        /// - `dto` must contain valid module name, route, and visibility settings.
        ///
        /// Postconditions:
        /// - Sends POST request to the backend and returns success flag.
        public async Task<bool> CreateModuleAsync(CreateModuleDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/admin/modules", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Updates an existing module based on the given data.
        /// </summary>
        /// Preconditions:
        /// - `dto` must contain a valid `Id` of an existing module and updated fields.
        ///
        /// Postconditions:
        /// - Sends PUT request to the backend and returns true if the update was successful.
        public async Task<bool> UpdateModuleAsync(UpdateModuleDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/admin/modules", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
