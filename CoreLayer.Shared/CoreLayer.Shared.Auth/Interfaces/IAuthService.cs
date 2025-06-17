using Microsoft.AspNetCore.Http;

namespace CoreLayer.Shared.Auth.Interfaces
{
    /// <summary>
    /// Defines authentication-related operations for user sign-in and sign-out.
    /// </summary>
    /// <remarks>
    /// This interface abstracts authentication logic, making it reusable across different contexts and implementations.
    /// </remarks>
    public interface IAuthService
    {
        /// <summary>
        /// Signs in a user by establishing an authentication session.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="role">The role assigned to the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SignInAsync(HttpContext context, string userId, string role);

        /// <summary>
        /// Signs out the currently authenticated user.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SignOutAsync(HttpContext context);
    }
}
