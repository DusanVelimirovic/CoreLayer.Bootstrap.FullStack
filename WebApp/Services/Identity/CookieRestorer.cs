using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Net;

namespace WebApp.Services.Identity
{
    /// <summary>
    /// Restores the authentication cookie from session or local storage into the shared CookieContainer.
    /// </summary>
    public class CookieRestorer
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly CookieContainer _cookieContainer;
        private readonly IConfiguration _configuration;
        private readonly IJSRuntime _jsRuntime;

        public CookieRestorer(
            ProtectedSessionStorage sessionStorage,
            CookieContainer cookieContainer,
            IConfiguration configuration,
            IJSRuntime jsRuntime)
        {
            _sessionStorage = sessionStorage;
            _cookieContainer = cookieContainer;
            _configuration = configuration;
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Attempts to restore the authentication cookie to the CookieContainer.
        /// </summary>
        /// Preconditions:
        /// - The cookie `.AspNetCore.Identity.Application` must exist in either session storage or local storage.
        /// - `ApiGateway:BaseUrl` must be defined in configuration.
        ///
        /// Postconditions:
        /// - If found, the cookie is added to the CookieContainer, enabling authenticated HTTP requests.
        /// - Logs are written to indicate source of restoration and success/failure.
        public async Task RestoreAuthCookieAsync()
        {
            try
            {
                string? cookieValue = null;

                // 1Try ProtectedSessionStorage first
                var result = await _sessionStorage.GetAsync<string>(".AspNetCore.Identity.Application");
                if (result.Success && !string.IsNullOrWhiteSpace(result.Value))
                {
                    cookieValue = result.Value;
                    Console.WriteLine("✅ Cookie restored from ProtectedSessionStorage.");
                }
                else
                {
                    // 2️⃣ Fallback to localStorage using JS interop
                    cookieValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ".AspNetCore.Identity.Application");
                    if (!string.IsNullOrWhiteSpace(cookieValue))
                    {
                        Console.WriteLine("⚠️ Cookie restored from localStorage (fallback).");
                    }
                }

                if (!string.IsNullOrWhiteSpace(cookieValue))
                {
                    var baseUrl = _configuration["ApiGateway:BaseUrl"];
                    if (string.IsNullOrWhiteSpace(baseUrl))
                    {
                        Console.WriteLine("❌ ApiGateway:BaseUrl is not configured.");
                        return;
                    }

                    var uri = new Uri(baseUrl);
                    var cookie = new Cookie(".AspNetCore.Identity.Application", cookieValue)
                    {
                        Domain = uri.Host,
                        Path = "/",
                        HttpOnly = false,
                        Secure = uri.Scheme == "https"
                    };

                    _cookieContainer.Add(uri, cookie);
                    Console.WriteLine("✅ .AspNetCore.Identity.Application added to CookieContainer.");
                }
                else
                {
                    Console.WriteLine("❌ No .AspNetCore.Identity.Application found to restore.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception during cookie restore: {ex.Message}");
            }
        }
    }
}
