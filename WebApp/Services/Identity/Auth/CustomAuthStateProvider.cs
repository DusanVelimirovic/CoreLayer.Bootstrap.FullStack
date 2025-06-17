using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using WebApp.DTOs.Identity;


namespace WebApp.Services.Identity.Auth
{
    /// <summary>
    /// Provides the current authentication state by reading from browser localStorage.
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private ClaimsPrincipal _anonymous => new(new ClaimsIdentity());

        /// <summary>
        /// Holds the current user session (user ID, roles, module access).
        /// </summary>
        public AuthUserSessionDto? CurrentSession { get; private set; }

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Gets the current authentication state from localStorage and builds a ClaimsPrincipal.
        /// </summary>
        /// Preconditions: "auth_user" must exist in browser localStorage in valid JSON format.
        /// Postconditions: Returns authenticated ClaimsPrincipal or anonymous if not found/invalid.
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "auth_user");

                if (string.IsNullOrEmpty(userJson))
                    return new AuthenticationState(_anonymous);

                var userData = JsonSerializer.Deserialize<AuthUserSessionDto>(userJson);

                if (userData == null || string.IsNullOrWhiteSpace(userData.UserId))
                    return new AuthenticationState(_anonymous);

                CurrentSession = userData;

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userData.UserId),
            new Claim(ClaimTypes.Name, userData.UserName)
        };

                // ✅ Add a claim for each role
                foreach (var roleId in userData.RoleIds)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleId));
                }

                // ✅ Optionally add module access claims
                foreach (var moduleId in userData.AccessibleModuleIds)
                {
                    claims.Add(new Claim("module", moduleId.ToString()));
                }

                var identity = new ClaimsIdentity(claims, "CustomAuth");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        /// <summary>
        /// Marks the user as authenticated by saving session and triggering auth state.
        /// </summary>
        /// Preconditions: Valid AuthUserSessionDto with non-null userId.
        /// Postconditions: Updates localStorage and triggers UI authentication refresh.
        public async Task MarkUserAsAuthenticated(AuthUserSessionDto session)
        {
            CurrentSession = session;

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "auth_user", JsonSerializer.Serialize(session));
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Logs out the current user by clearing localStorage and setting auth state to anonymous.
        /// </summary>
        /// Preconditions: "auth_user" must exist in localStorage.
        /// Postconditions: localStorage cleared, UI switched to unauthenticated state.
        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "auth_user");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
        
        /// <summary>
        /// Triggers a manual re-evaluation of the authentication state.
        /// </summary>
        /// Preconditions: None (can be called at any time).
        /// Postconditions: Forces Blazor to re-check authentication state.
        public void NotifyAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
