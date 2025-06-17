using System.Net;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using WebApp.DTOs.Identity;

namespace WebApp.Services.Identity.Auth
{
    /// <summary>
    /// Handles authentication, session storage, cookie persistence, and 2FA flows.
    /// </summary>
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly CookieContainer _cookieContainer;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthService(HttpClient client,
                           IJSRuntime jsRuntime,
                           IHttpContextAccessor httpContextAccessor,
                           ProtectedSessionStorage sessionStorage,
                           CookieContainer cookieContainer,
                           CustomAuthStateProvider authStateProvider)
        {
            _httpClient = client;
            _jsRuntime = jsRuntime;
            _sessionStorage = sessionStorage;
            _cookieContainer = cookieContainer;
            _authStateProvider = authStateProvider;
        }

        private bool _isAuthenticated;
        public bool IsAuthenticated => _isAuthenticated;

        public event Action? AuthStateChanged;
        private void NotifyAuthChanged() => AuthStateChanged?.Invoke();

        /// <summary>
        /// Attempts user login via API and stores auth cookies.
        /// </summary>
        /// Preconditions: Valid LoginRequestDto with credentials.
        /// Postconditions: Identity cookie saved; session and local storage updated; auth state triggered.
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", dto);
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                // Try to read cookie headers from response (for debugging)
                if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
                {
                    foreach (var header in cookieHeaders)
                    {
                        Console.WriteLine($"[AuthService] Set-Cookie header: {header}");
                    }

                    var cookieHeader = cookieHeaders.FirstOrDefault(h => h.StartsWith(".AspNetCore.Identity.Application="));
                    if (cookieHeader != null)
                    {
                        Console.WriteLine($"[AuthService] .AspNetCore.Identity.Application found: {cookieHeader}");
                        await _jsRuntime.InvokeVoidAsync("storeCookie", cookieHeader); // optional JS storage
                    }
                    else
                    {
                        Console.WriteLine("[AuthService] .AspNetCore.Identity.Application not found in Set-Cookie headers.");
                    }
                }

                // Extract TrlicAuthCookie from CookieContainer
                var baseUri = new Uri(_httpClient.BaseAddress!.ToString());
                var cookies = _cookieContainer.GetCookies(baseUri);

                Console.WriteLine("[DEBUG][Frontend] Cookies being sent on future requests:");

                foreach (Cookie cookie in cookies)
                {
                    if (cookie.Name == ".AspNetCore.Identity.Application")
                    {
                        await _sessionStorage.SetAsync(".AspNetCore.Identity.Application", cookie.Value);

                        // FIX: Delay JS injection to ensure document and JS environment are fully ready
                        await Task.Delay(200); // Give browser time to fully boot up
                        await _jsRuntime.InvokeVoidAsync("storeCookie", $".AspNetCore.Identity.Application={cookie.Value}");
                        Console.WriteLine("✅ JSInterop storeCookie executed successfully.");
                        Console.WriteLine($"[DEBUG][Frontend] {cookie.Name} = {cookie.Value}; Domain: {cookie.Domain}");
                    }
                }

                _isAuthenticated = true;
                NotifyAuthChanged();
            }

            return result;
        }

        /// <summary>
        /// Logs the user out and clears authentication data from all storages.
        /// </summary>
        /// Preconditions: Valid session and identity cookie available.
        /// Postconditions: Session cleared, auth state reset, localStorage and cookies removed.
        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("/api/auth/logout", null);

                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "pending2fa_userId");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "trustedDevice");

                await _jsRuntime.InvokeVoidAsync("clearAuthCookie"); // browser cookie cleanup (optional)
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", ".AspNetCore.Identity.Application");
                await _sessionStorage.DeleteAsync(".AspNetCore.Identity.Application"); // ✅ session cleanup

                _isAuthenticated = false;
                NotifyAuthChanged();

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Submits a 2FA verification token for authentication.
        /// </summary>
        /// Preconditions: Token and userId must be populated in DTO.
        /// Postconditions: Success flag and server-side validation message returned.
        public async Task<(bool Success, string? Message)> VerifyTwoFactorAsync(TwoFactorVerificationDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/verify-2fa", dto);
            var result = await response.Content.ReadFromJsonAsync<GenericApiResponseDto>();
            return (response.IsSuccessStatusCode, result?.Message);
        }

        /// <summary>
        /// Marks a device as trusted for the given user.
        /// </summary>
        /// Preconditions: User must be authenticated and device identifier provided.
        /// Postconditions: Server stores trusted device status.
        public async Task<bool> MarkDeviceAsTrustedAsync(string userId, string deviceIdentifier)
        {
            var dto = new TrustedDeviceRequestDto
            {
                UserId = userId,
                DeviceIdentifier = deviceIdentifier
            };

            var response = await _httpClient.PostAsJsonAsync("/api/auth/trust-device", dto);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Requests a password reset by sending reset instructions to the user's email.
        /// </summary>
        /// Preconditions: Valid email provided via DTO.
        /// Postconditions: Reset link sent to user (if email exists).
        public async Task<bool> RequestPasswordResetAsync(ForgotPasswordRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/request-password-reset", dto);
                var json = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in RequestPasswordResetAsync: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Completes password reset with token and new password.
        /// </summary>
        /// Preconditions: Valid token and new password provided via DTO.
        /// Postconditions: User password changed or error message returned.
        public async Task<(bool Success, string? Message)> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", dto);
                var result = await response.Content.ReadFromJsonAsync<GenericApiResponseDto>();
                return (response.IsSuccessStatusCode, result?.Message ?? "Unknown response");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception in ResetPasswordAsync: {ex.Message}");
                return (false, "Error occurred while resetting password.");
            }
        }

         /// <summary>
        /// Completes login after successful 2FA verification and restores user session.
        /// </summary>
        /// 🔒 Preconditions: Valid userId must be passed after 2FA success.
        /// 📤 Postconditions: Session data restored, identity cookie saved, user marked as authenticated.
        public async Task<bool> LoginAfter2FAConfirmedAsync(string userId)
        {
            var dto = new TwoFactorLoginRequestDto { UserId = userId };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login-after-2fa", dto);
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                var baseUri = new Uri(_httpClient.BaseAddress!.ToString());
                var cookies = _cookieContainer.GetCookies(baseUri);

                foreach (Cookie cookie in cookies)
                {
                    if (cookie.Name == ".AspNetCore.Identity.Application")
                    {
                        await _sessionStorage.SetAsync(".AspNetCore.Identity.Application", cookie.Value);
                        break;
                    }
                }

                // ✅ NEW: Set auth_user session!
                var session = new AuthUserSessionDto
                {
                    UserId = result.UserId!,
                    UserName = "[2FA User]", // optionally load real name from another source
                    RoleIds = result.RoleIds ?? new List<string>(),
                    AccessibleModuleIds = result.ModuleIdsWithAccess ?? new List<int>(),

                };

                // You MUST inject AuthStateProvider and call this:
                if (_authStateProvider is CustomAuthStateProvider customAuth)
                {
                    await customAuth.MarkUserAsAuthenticated(session);
                }

                _isAuthenticated = true;
                NotifyAuthChanged();
                return true;
            }
            return false;
        }
    }
}
