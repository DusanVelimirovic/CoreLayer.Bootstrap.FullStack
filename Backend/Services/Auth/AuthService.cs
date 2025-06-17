using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Backend.Services.Interfaces.Email;
using Backend.Data;
using Backend.Services.Interfaces.Auth;

namespace Backend.Services.Auth
{
    /// <summary>
    /// Service responsible for user authentication, login auditing, 2FA handling,
    /// trusted device registration, and password reset functionality.
    /// </summary>
    /// <remarks>
    /// Implements IAuthService to encapsulate identity and security logic.
    /// </remarks>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CoreLayerDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CoreLayerDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _logger = logger;
        }

        private const int MaxFailedAttempts = 10;
        private readonly TimeSpan FailedLoginWindow = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Authenticates a user with credentials, checks for 2FA, trusted device,
        /// and records login attempt audit.
        /// </summary>
        /// <param name="loginDto">The login credentials and device information.</param>
        /// <returns>
        /// A <see cref="LoginResponseDto"/> indicating success or failure, along with
        /// user role/module info or 2FA requirement if applicable.
        /// </returns>

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var userAgent = GetUserAgent();
            var deviceName = loginDto.DeviceName;
            var deviceIdentifier = loginDto.DeviceIdentifier;
            var ip = GetIpAddress();
            var trustedDeviceUsed = false;

            var user = await _userManager.FindByNameAsync(loginDto.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(loginDto.UserNameOrEmail);


            // Log all incoming cookies from the request
            if (_httpContextAccessor.HttpContext?.Request?.Cookies is { Count: > 0 } cookies)
            {
                foreach (var cookie in cookies)
                {
                    _logger.LogInformation($"[DEBUG][Backend] Cookie received: {cookie.Key} = {cookie.Value}");
                }
            }
            else
            {
                _logger.LogWarning("[DEBUG][Backend] No cookies received in the request.");
            }


            // Handle user not found or inactive
            if (user == null)
            {
                await SaveLoginAttemptAsync(
                    userId: "unknown",
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: "User not found",
                    is2FASuccess: null,
                    deviceName: deviceName,
                    isTrustedDevice: false);

                return new LoginResponseDto { Success = false, Message = "Invalid credentials." };
            }

            if (!user.IsActive)
            {
                await SaveLoginAttemptAsync(
                    userId: user.Id,
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: "Account inactive",
                    is2FASuccess: null,
                    deviceName: deviceName,
                    isTrustedDevice: false);

                return new LoginResponseDto { Success = false, Message = "Invalid credentials." };
            }

            // Move this line out of LINQ
            var windowStart = DateTime.UtcNow - FailedLoginWindow;

            // Soft brute-force protection check
            var recentFailures = await _context.LoginAuditLogs
                .Where(l =>
                    l.UserId == user.Id &&
                    !l.Success &&
                    l.LoginTime > windowStart)
                .CountAsync();

            if (recentFailures >= MaxFailedAttempts)
            {
                await SaveLoginAttemptAsync(
                    userId: user.Id,
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: $"Too many failed attempts ({recentFailures} in {FailedLoginWindow.TotalMinutes} min)",
                    is2FASuccess: null,
                    deviceName: deviceName,
                    isTrustedDevice: false);

                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Too many failed login attempts. Please wait a few minutes and try again."
                };
            }


            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                string reason = result.IsLockedOut ? "Account locked" : "Invalid password";

                var attemptsLeft = MaxFailedAttempts - recentFailures - 1;

                await SaveLoginAttemptAsync(
                    userId: user.Id,
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: reason,
                    is2FASuccess: null,
                    deviceName: deviceName,
                    isTrustedDevice: false);

                return new LoginResponseDto
                {
                    Success = false,
                    Message = reason + (attemptsLeft > 0 ? $" ({attemptsLeft} attempts left before account lock)" : ""),
                    RemainingAttempts = attemptsLeft
                };
            }

            // Fetch role and module access info (for all valid attempts)
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

            // 2FA check
            if (result.Succeeded && user.TwoFactorEnabled)
            {
                if (!string.IsNullOrWhiteSpace(deviceIdentifier))
                {
                    trustedDeviceUsed = await _context.TrustedDevices.AnyAsync(td =>
                        td.UserId == user.Id &&
                        td.DeviceIdentifier == deviceIdentifier &&
                        td.ExpiresOn > DateTime.UtcNow);

                    if (trustedDeviceUsed)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        await SaveLoginAttemptAsync(
                            userId: user.Id,
                            ipAddress: ip,
                            success: true,
                            userAgent: userAgent,
                            is2FASuccess: true,
                            deviceName: deviceName,
                            isTrustedDevice: true);

                        return new LoginResponseDto
                        {
                            Success = true,
                            Message = "Trusted device. 2FA skipped.",
                            UserId = user.Id,
                            RoleNames = roles.ToList(),
                            RoleIds = roleIds,
                            ModuleIdsWithAccess = accessibleModules
                        };
                    }
                }

                // 2FA needed — issue token and audit
                await GenerateAndStoreTwoFactorTokenAsync(user);

                await SaveLoginAttemptAsync(
                    userId: user.Id,
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: "2FA required",
                    is2FASuccess: false,
                    deviceName: deviceName,
                    isTrustedDevice: false);

                return new LoginResponseDto
                {
                    Success = false,
                    RequiresTwoFactor = true,
                    Message = "2FA required. Check your email for the code.",
                    UserId = user.Id
                };
            }

            // ✅ Normal login success (no 2FA)
            await _signInManager.SignInAsync(user, isPersistent: true);

            await SaveLoginAttemptAsync(
                userId: user.Id,
                ipAddress: ip,
                success: true,
                userAgent: userAgent,
                is2FASuccess: null,
                deviceName: deviceName,
                isTrustedDevice: trustedDeviceUsed);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful.",
                UserId = user.Id,
                RoleNames = roles.ToList(),
                RoleIds = roleIds,
                ModuleIdsWithAccess = accessibleModules
            };
        }

        /// <summary>
        /// Persists a login attempt to the audit log, capturing success, failure reasons,
        /// device info, and 2FA outcomes.
        /// </summary>
        /// <param name="userId">User ID (or "unknown").</param>
        /// <param name="ipAddress">IP address of the request.</param>
        /// <param name="success">Whether the login was successful.</param>
        /// <param name="userAgent">User-Agent header (browser/device info).</param>
        /// <param name="failureReason">Optional failure explanation.</param>
        /// <param name="is2FASuccess">Whether 2FA succeeded, failed, or was not used.</param>
        /// <param name="deviceName">Optional device name used during login.</param>
        /// <param name="isTrustedDevice">Whether login used a trusted device.</param>

        public async Task SaveLoginAttemptAsync(
            string userId,
            string? ipAddress,
            bool success,
            string? userAgent = null,
            string? failureReason = null,
            bool? is2FASuccess = null,
            string? deviceName = null,
            bool? isTrustedDevice = null)
        {
            var log = new LoginAuditLog
            {
                UserId = userId,
                IpAddress = ipAddress,
                Success = success,
                LoginTime = DateTime.UtcNow,
                UserAgent = userAgent,
                FailureReason = failureReason,
                Is2FASuccess = is2FASuccess,
                DeviceName = deviceName,
                IsTrustedDevice = isTrustedDevice
            };

            _context.LoginAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Verifies a 2FA token for a given user and updates audit log accordingly.
        /// </summary>
        /// <param name="userId">ID of the user being verified.</param>
        /// <param name="token">2FA token to verify.</param>
        /// <returns>True if token is valid and verified; otherwise false.</returns>

        public async Task<bool> VerifyTwoFactorAsync(string userId, string token)
        {
            var ip = GetIpAddress();
            var userAgent = ""; // Optionally extract in future
            string? failureReason = null;

            var validToken = await _context.TwoFactorTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Token == token && t.Expiry > DateTime.UtcNow);

            var user = await _userManager.FindByIdAsync(userId);

            if (validToken == null)
            {
                failureReason = "Invalid or expired 2FA token";

                // Log 2FA failure
                await SaveLoginAttemptAsync(
                    userId: userId,
                    ipAddress: ip,
                    success: false,
                    userAgent: userAgent,
                    failureReason: failureReason,
                    is2FASuccess: false,
                    deviceName: null,
                    isTrustedDevice: false);

                return false;
            }

            // Valid token → remove from DB
            _context.TwoFactorTokens.Remove(validToken);
            await _context.SaveChangesAsync();

            // Log 2FA success
            await SaveLoginAttemptAsync(
                userId: userId,
                ipAddress: ip,
                success: true,
                userAgent: userAgent,
                failureReason: null,
                is2FASuccess: true,
                deviceName: null,
                isTrustedDevice: false); // 2FA = not yet trusted

            return true;
        }

        private string? GetIpAddress()
        {
            try
            {
                return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private string? GetUserAgent()
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
        }

        // Generate 2FA token
        private async Task GenerateAndStoreTwoFactorTokenAsync(ApplicationUser user)
        {
            var token = new TwoFactorToken
            {
                UserId = user.Id,
                Token = new Random().Next(100000, 999999).ToString(), // e.g., 843122
                Expiry = DateTime.UtcNow.AddMinutes(10)
            };

            _context.TwoFactorTokens.Add(token);
            await _context.SaveChangesAsync();

            // Simulate email
            // Console.WriteLine($"[2FA EMAIL SIMULATION] Send to: {user.Email}, Code: {token.Token}");
            var emailSuccess = await _emailService.Send2FATokenAsync(user.Email!, token.Token);
            await LogEmailAuditAsync(user.Email!, "2FA", emailSuccess, emailSuccess ? null : "Failed to send 2FA email");
        }

        /// <summary>
        /// Marks a device as trusted for a specific user, bypassing 2FA on future logins
        /// from this device within expiration.
        /// </summary>
        /// <param name="dto">Trusted device data (user ID and device identifier).</param>
        /// <returns>True if device was marked as trusted or already trusted; false otherwise.</returns>
        public async Task<bool> MarkDeviceAsTrustedAsync(TrustedDeviceRequestDto dto)
        {
            // Check if already exists
            var exists = await _context.TrustedDevices
                .AnyAsync(td => td.UserId == dto.UserId && td.DeviceIdentifier == dto.DeviceIdentifier);

            if (exists)
            {
                // Optional: log duplicate trust request if needed
                return true;
            }

            var trustedDevice = new TrustedDevice
            {
                UserId = dto.UserId,
                DeviceIdentifier = dto.DeviceIdentifier,
                TrustedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(30)
            };

            _context.TrustedDevices.Add(trustedDevice);

            // ✅ Log event in LoginAuditLog
            await SaveLoginAttemptAsync(
                userId: dto.UserId,
                ipAddress: GetIpAddress(),
                success: true,
                userAgent: GetUserAgent(),
                failureReason: null,
                is2FASuccess: true,
                deviceName: dto.DeviceName,
                isTrustedDevice: true // <- this is key
            );

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a paginated list of login audit logs based on optional filters like user ID,
        /// success state, or date range.
        /// </summary>
        /// <param name="query">Filtering and pagination parameters.</param>
        /// <returns>List of <see cref="LoginAuditLogResultDto"/> entries.</returns>
        public async Task<List<LoginAuditLogResultDto>> GetLoginAuditLogsAsync(LoginAuditLogQueryDto query)
        {
            var logsQuery = _context.LoginAuditLogs
                .Include(l => l.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.UserIdOrEmail))
            {
                logsQuery = logsQuery.Where(l =>
                    l.UserId == query.UserIdOrEmail ||
                    l.User!.Email!.Contains(query.UserIdOrEmail));
            }

            if (query.Success.HasValue)
                logsQuery = logsQuery.Where(l => l.Success == query.Success.Value);

            if (query.From.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(query.From.Value.Date, DateTimeKind.Utc);
                logsQuery = logsQuery.Where(l => l.LoginTime >= fromUtc);
            }

            if (query.To.HasValue)
            {
                var toUtc = DateTime.SpecifyKind(query.To.Value.Date, DateTimeKind.Utc);
                logsQuery = logsQuery.Where(l => l.LoginTime <= toUtc);
            }


            var skip = (query.Page - 1) * query.PageSize;

            var results = await logsQuery
                .OrderByDescending(l => l.LoginTime)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(l => new LoginAuditLogResultDto
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    UserEmail = l.User!.Email,
                    Success = l.Success,
                    FailureReason = l.FailureReason,
                    IpAddress = l.IpAddress,
                    UserAgent = l.UserAgent,
                    DeviceName = l.DeviceName,
                    Is2FASuccess = l.Is2FASuccess,
                    IsTrustedDevice = l.IsTrustedDevice,
                    LoginTime = l.LoginTime
                })
                .ToListAsync();

            return results;
        }

        /// <summary>
        /// Generates and sends a password reset token to the user's confirmed email address.
        /// </summary>
        /// <param name="email">The email of the user requesting a reset.</param>
        /// <returns>True if the token was successfully generated and email sent; otherwise false.</returns>
        public async Task<bool> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
                return false;

            // Mark old tokens as used
            var oldTokens = await _context.PasswordResetTokens
                .Where(t => t.UserId == user.Id && !t.IsUsed && t.ExpirationTime > DateTime.UtcNow)
                .ToListAsync();

            foreach (var old in oldTokens)
                old.IsUsed = true;

            var token = Guid.NewGuid().ToString();

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpirationTime = DateTime.UtcNow.AddMinutes(30)
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            // Simulate email
            // Console.WriteLine( $"[RESET EMAIL SIMULATION] To: {user.Email} | Token: {token}");
            Console.WriteLine($"[DEBUG] Attempting to send reset token to {user.Email}"); // <== Add this
            var emailSuccess = await _emailService.SendPasswordResetTokenAsync(user.Email!, token);
            await LogPasswordResetAuditAsync(user.Id, "Password reset requested", true);
            await LogEmailAuditAsync(user.Email!, "PasswordReset", emailSuccess, emailSuccess ? null : "Failed to send reset token");

            return true;
        }

        /// <summary>
        /// Validates whether a password reset token is valid, unexpired, and unused.
        /// </summary>
        /// <param name="userId">ID of the user requesting reset.</param>
        /// <param name="token">The token to validate.</param>
        /// <returns>True if the token is valid; otherwise false.</returns>
        public async Task<bool> ValidatePasswordResetTokenAsync(string userId, string token)
        {
            var resetToken = await _context.PasswordResetTokens
                .Where(t => t.UserId == userId && t.Token == token)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync();

            if (resetToken == null || resetToken.IsUsed || resetToken.ExpirationTime < DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets a user's password using a previously validated reset token.
        /// </summary>
        /// <param name="dto">The reset request containing email, token, and new password.</param>
        /// <returns>True if the reset was successful; otherwise false.</returns>
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return false;

            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t =>
                    t.UserId == user.Id &&
                    t.Token == dto.Token &&
                    t.ExpirationTime > DateTime.UtcNow &&
                    !t.IsUsed);

            if (resetToken == null)
                return false;

            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, identityToken, dto.NewPassword);
            if (!result.Succeeded)
            {
                await LogPasswordResetAuditAsync(user.Id, "Invalid or expired password reset token", false);
                return false;
            }

            resetToken.IsUsed = true;
            await _context.SaveChangesAsync();

            await LogPasswordResetAuditAsync(user.Id, "Password reset completed", true);

            return true;
        }

        /// <summary>
        /// Logs an event related to a password reset action into the login audit log.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <param name="message">Details of the reset event.</param>
        /// <param name="success">Indicates whether the operation succeeded.</param>
        private async Task LogPasswordResetAuditAsync(string userId, string message, bool success)
        {
            var log = new LoginAuditLog
            {
                UserId = userId,
                IpAddress = GetIpAddress(),
                Success = success,
                LoginTime = DateTime.UtcNow,
                UserAgent = GetUserAgent(),
                FailureReason = message,
                Is2FASuccess = null,
                DeviceName = null,
                IsTrustedDevice = null
            };

            _context.LoginAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Logs the result of an email operation, such as sending 2FA or reset token emails.
        /// </summary>
        /// <param name="toEmail">Recipient email address.</param>
        /// <param name="templateType">Type of email sent (e.g., 2FA, PasswordReset).</param>
        /// <param name="success">True if email was sent successfully.</param>
        /// <param name="errorMessage">Optional failure reason.</param>
        private async Task LogEmailAuditAsync(string toEmail, string templateType, bool success, string? errorMessage = null)
        {
            var log = new EmailAuditLog
            {
                ToEmail = toEmail,
                TemplateType = templateType,
                Success = success,
                SentAt = DateTime.UtcNow,
                ErrorMessage = errorMessage
            };

            _context.EmailAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
