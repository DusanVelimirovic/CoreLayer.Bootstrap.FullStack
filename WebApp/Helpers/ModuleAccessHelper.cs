using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using WebApp.Services.Identity.Auth;


namespace WebApp.Helpers
{
    /// <summary>
    /// Provides helper methods to manage and evaluate access to application modules based on the authenticated user's permissions.
    /// </summary>
    /// <remarks>
    /// This class interacts with the authentication state provider and uses cookies, local storage, and the backend API
    /// to evaluate and refresh module access rights for the current session.
    /// </remarks>
    public class ModuleAccessHelper
    {
        private readonly CustomAuthStateProvider _authStateProvider;
        private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly CookieContainer _cookieContainer;
        private readonly Uri _apiBaseUri;
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleAccessHelper"/> class.
        /// </summary>
        /// <param name="authStateProvider">The custom authentication state provider.</param>
        /// <param name="httpClient">The HTTP client used for API requests.</param>
        /// <param name="sessionStorage">Browser-protected session storage.</param>
        /// <param name="cookieContainer">Container holding identity cookies for API requests.</param>
        /// <param name="config">Application configuration providing the API base URL.</param>
        /// <param name="jsRuntime">JavaScript runtime used to interact with localStorage.</param>
        public ModuleAccessHelper(
            CustomAuthStateProvider authStateProvider,
            HttpClient httpClient,
            ProtectedSessionStorage sessionStorage,
            CookieContainer cookieContainer,
            IConfiguration config,
            IJSRuntime jsRuntime)
        {
            _authStateProvider = authStateProvider;
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
            _cookieContainer = cookieContainer;
            _apiBaseUri = new Uri(config["ApiGateway:BaseUrl"]!);
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Checks whether the currently authenticated user has access to a specified module.
        /// </summary>
        /// <param name="moduleId">The ID of the module to check access for.</param>
        /// <returns><c>true</c> if the user has access; otherwise, <c>false</c>.</returns>
        public bool HasAccess(int moduleId)
        {
            return _authStateProvider.CurrentSession?.AccessibleModuleIds.Contains(moduleId) ?? false;
        }

        /// <summary>
        /// Refreshes the list of accessible module IDs from the backend for the current user session.
        /// </summary>
        /// <returns><c>true</c> if the access list was successfully refreshed; otherwise, <c>false</c>.</returns>
        public async Task<bool> RefreshModuleAccessAsync()
        {
            try
            {
                var cookies = _cookieContainer.GetCookies(_apiBaseUri);
                var identityCookie = cookies[".AspNetCore.Identity.Application"];
                if (identityCookie == null || string.IsNullOrWhiteSpace(identityCookie.Value))
                {
                    Console.WriteLine("[ModuleAccessHelper] Skipped refresh — not authenticated yet.");
                    return false;
                }
                var rawJson = await _httpClient.GetStringAsync("/api/auth/me/modules");

                var parsed = JsonDocument.Parse(rawJson);
                var updatedModules = parsed.RootElement
                    .GetProperty("moduleIds")
                    .EnumerateArray()
                    .Select(x => x.GetInt32())
                    .ToList();

                if (_authStateProvider.CurrentSession != null)
                {
                    _authStateProvider.CurrentSession.AccessibleModuleIds = updatedModules;

                    // Update localStorage
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                        "auth_user", JsonSerializer.Serialize(_authStateProvider.CurrentSession));

                    _authStateProvider.NotifyAuthStateChanged();
                }


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ModuleAccessHelper] Failed to refresh module access: {ex.Message}");
                return false;
            }
        }
    }
}
