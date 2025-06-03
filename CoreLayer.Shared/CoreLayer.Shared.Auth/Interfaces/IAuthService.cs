using Microsoft.AspNetCore.Http;

namespace CoreLayer.Shared.Auth.Interfaces
{
    public interface IAuthService
    {
        Task SignInAsync(HttpContext context, string userId, string role);
        Task SignOutAsync(HttpContext context);
    }
}
