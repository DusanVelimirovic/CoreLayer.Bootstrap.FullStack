using Backend.DTOs.Auth;
using CoreLayer.Shared.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 🔐 Simulate login check (this is just a stub — replace with real DB logic later)
        if (request.Username == "dusan" && request.Password == "123")
        {
            await _authService.SignInAsync(HttpContext, userId: "1", role: "Admin");
            return Ok(new { message = "Login successful" });
        }

        return Unauthorized(new { message = "Invalid credentials" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync(HttpContext);
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    public IActionResult WhoAmI()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
            return Unauthorized();

        return Ok(new
        {
            userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
            role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}
