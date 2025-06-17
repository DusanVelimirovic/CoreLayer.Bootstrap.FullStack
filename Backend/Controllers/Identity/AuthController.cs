using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backend.Models.Identity;
using Backend.DTOs.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Services.Auth;
using Backend.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers.Identity
{
    /// <summary>
    /// Handles authentication, authorization, and identity-related operations.
    /// </summary>
    /// <remarks>
    /// Includes login, logout, 2FA verification, password reset, and module access queries.
    /// </remarks>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CoreLayerDbContext _context;
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            CoreLayerDbContext context,
            IAuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user with provided credentials.
        /// </summary>
        /// <param name="loginDto">Login credentials.</param>
        /// <returns>Login response with success status or two-factor prompt.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            if (!response.Success && !response.RequiresTwoFactor)
                return Unauthorized(response);

            // 🧪 Log the Set-Cookie header if it was written
            if (HttpContext.Response.Headers.TryGetValue("Set-Cookie", out var setCookie))
            {
                Console.WriteLine("Set-Cookie issued: " + setCookie.ToString());
            }
            else
            {
                Console.WriteLine("No Set-Cookie header was issued in response.");
            }

            return Ok(response);
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>Success status.</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User?.Identity?.IsAuthenticated == true
                ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var ip = GetIpAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();

                await _authService.SaveLoginAttemptAsync(
                    userId: userId,
                    ipAddress: ip,
                    success: true,
                    userAgent: userAgent,
                    failureReason: null,
                    is2FASuccess: null,
                    deviceName: null,
                    isTrustedDevice: null
                );
            }

            await _signInManager.SignOutAsync();

            // Manually overwrite the auth cookie with an expired one
            Response.Cookies.Append(".AspNetCore.Identity.Application", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/"
            });

            return Ok(new { success = true, message = "Logged out successfully." });
        }

        private string? GetIpAddress()
        {
            try
            {
                return HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Verifies a two-factor authentication token.
        /// </summary>
        /// <param name="dto">Two-factor verification data.</param>
        /// <returns>Success or failure message.</returns>
        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerificationDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null || !user.TwoFactorEnabled)
                return Unauthorized(new { success = false, message = "Invalid or expired 2FA session." });

            var success = await _authService.VerifyTwoFactorAsync(user.Id, dto.Token);
            if (!success)
                return Unauthorized(new { success = false, message = "Invalid or expired 2FA token." });

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { success = true, message = "Two-factor authentication successful." });
        }

        /// <summary>
        /// Marks a device as trusted for a user.
        /// </summary>
        /// <param name="dto">Trusted device request payload.</param>
        /// <returns>Success status and message.</returns>
        [HttpPost("trust-device")]
        public async Task<IActionResult> TrustDevice([FromBody] TrustedDeviceRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null || !user.TwoFactorEnabled)
                return Unauthorized(new { success = false, message = "User not found or 2FA not enabled." });

            var result = await _authService.MarkDeviceAsTrustedAsync(dto);
            return Ok(new { success = result, message = result ? "Device trusted." : "Already trusted." });
        }

        /// <summary>
        /// Cleans up expired 2FA tokens.
        /// </summary>
        /// <param name="cleanupService">Injected 2FA cleanup service.</param>
        /// <returns>Count of tokens removed.</returns>
        [HttpPost("cleanup-expired-2fa-tokens")]
        public async Task<IActionResult> Cleanup2FATokens(
            [FromServices] TwoFactorCleanupService cleanupService)
        {
            var count = await cleanupService.CleanupExpiredTokensAsync();
            return Ok(new { success = true, message = $"{count} expired 2FA tokens removed." });
        }

        /// <summary>
        /// Sends a password reset link if the email is valid.
        /// </summary>
        /// <param name="dto">Password reset request data.</param>
        /// <returns>Generic success message for security.</returns>
        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto dto)
        {
            var result = await _authService.GeneratePasswordResetTokenAsync(dto.Email);

            return Ok(new
            {
                success = result,
                message = result
                    ? "If your email exists in our system, you'll receive a password reset link shortly."
                    : "If your email exists in our system, you'll receive a password reset link shortly." // keep vague for security
            });
        }

        /// <summary>
        /// Validates a password reset token.
        /// </summary>
        /// <param name="dto">Reset token validation data.</param>
        /// <returns>Validation result.</returns>
        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequestDto dto)
        {
            var isValid = await _authService.ValidatePasswordResetTokenAsync(dto.UserId, dto.Token);

            if (!isValid)
                return BadRequest(new { success = false, message = "Invalid or expired token." });

            return Ok(new { success = true, message = "Token is valid." });
        }

        /// <summary>
        /// Resets a user's password using a valid token.
        /// </summary>
        /// <param name="dto">Password reset data.</param>
        /// <returns>Success or failure response.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var success = await _authService.ResetPasswordAsync(dto);
            if (!success)
                return BadRequest(new { success = false, message = "Invalid or expired token." });

            return Ok(new { success = true, message = "Password reset successful." });
        }

        /// <summary>
        /// Completes login process after successful 2FA.
        /// </summary>
        /// <param name="dto">Two-factor login request.</param>
        /// <returns>Login response DTO with roles and module access.</returns>
        [HttpPost("login-after-2fa")]
        public async Task<IActionResult> LoginAfter2FA([FromBody] TwoFactorLoginRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
                return Unauthorized(new { success = false, message = "User not found." });

            await _signInManager.SignInAsync(user, isPersistent: false);

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = await _context.UserRoles
                .Where(r => r.UserId == user.Id)
                .Select(r => r.RoleId)
                .ToListAsync();

            var accessibleModules = await _context.ModuleAccessControls
                .Where(ac => roleIds.Contains(ac.RoleId) && ac.CanAccess)
                .Select(ac => ac.ModuleId)
                .Distinct()
                .ToListAsync();

            return Ok(new LoginResponseDto
            {
                Success = true,
                Message = "Login via 2FA complete.",
                UserId = user.Id,
                RoleNames = roles.ToList(),
                RoleIds = roleIds,
                ModuleIdsWithAccess = accessibleModules
            });
        }

        /// <summary>
        /// Retrieves all module IDs that the current authenticated user has access to.
        /// </summary>
        /// <returns>List of module IDs.</returns>
        [Authorize]
        [HttpGet("me/modules")]
        public async Task<IActionResult> GetCurrentUserModules()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
                return Unauthorized();

            var roleIds = await _context.UserRoles
                .Where(r => r.UserId == userId)
                .Select(r => r.RoleId)
                .ToListAsync();

            var accessibleModules = await _context.ModuleAccessControls
                .Where(ac => roleIds.Contains(ac.RoleId) && ac.CanAccess)
                .Select(ac => ac.ModuleId)
                .Distinct()
                .ToListAsync();

            return Ok(new { moduleIds = accessibleModules });
        }
    }
}
