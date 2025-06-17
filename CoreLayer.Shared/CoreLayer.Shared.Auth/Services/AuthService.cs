using CoreLayer.Shared.Auth.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http; // ✅ This line is critical

namespace CoreLayer.Shared.Auth.Services
{
    /// <summary>
    /// Provides cookie-based authentication services for signing users in and out.
    /// </summary>
    /// <remarks>
    /// This implementation uses the <see cref="CookieAuthenticationDefaults.AuthenticationScheme"/> 
    /// to establish or 
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Signs in a user by creating a claims principal and setting an authentication cookie.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="role">The role assigned to the user.</param>
        /// <returns>A task representing the asynchronous sign-in operation.</returns>
        public async Task SignInAsync(HttpContext context, string userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        /// <summary>
        /// Signs out the currently authenticated user by clearing the authentication cookie.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous sign-out operation.</returns>
        public async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
