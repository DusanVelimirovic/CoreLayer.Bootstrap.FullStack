using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Auth
{
    /// <summary>
    /// Defines authentication-related operations such as login, 2FA, password reset, and auditing.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Attempts to log in a user using the provided credentials and handles optional 2FA and auditing.
        /// </summary>
        /// <param name="loginDto">Login request details including username/email and password.</param>
        /// <returns>A <see cref="LoginResponseDto"/> containing login result and additional user info.</returns>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);

        /// <summary>
        /// Logs the result of a login attempt for auditing purposes.
        /// </summary>
        /// <param name="userId">The ID of the user attempting to log in.</param>
        /// <param name="ipAddress">IP address of the requester.</param>
        /// <param name="success">Whether the login attempt was successful.</param>
        /// <param name="userAgent">User agent string from the client (optional).</param>
        /// <param name="failureReason">Reason for failure, if applicable.</param>
        /// <param name="is2FASuccess">Indicates whether 2FA was successful, if applicable.</param>
        /// <param name="deviceName">Name of the device used during login (optional).</param>
        /// <param name="isTrustedDevice">Indicates if the login used a trusted device.</param>
        Task SaveLoginAttemptAsync(
            string userId,
            string? ipAddress,
            bool success,
            string? userAgent,
            string? failureReason,
            bool? is2FASuccess,
            string? deviceName,
            bool? isTrustedDevice);

        /// <summary>
        /// Verifies a submitted 2FA token for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user submitting the token.</param>
        /// <param name="token">The 2FA token provided by the user.</param>
        /// <returns><c>true</c> if the token is valid and not expired; otherwise <c>false</c>.</returns>
        Task<bool> VerifyTwoFactorAsync(string userId, string token);

        /// <summary>
        /// Marks a device as trusted for the given user, skipping future 2FA on that device.
        /// </summary>
        /// <param name="dto">Trusted device details including user ID and device identifier.</param>
        /// <returns><c>true</c> if the device is successfully marked; otherwise <c>false</c>.</returns>
        Task<bool> MarkDeviceAsTrustedAsync(TrustedDeviceRequestDto dto);

        /// <summary>
        /// Retrieves a paginated and optionally filtered list of login audit logs.
        /// </summary>
        /// <param name="queryDto">Audit log query parameters including date range, user filter, and pagination.</param>
        /// <returns>List of <see cref="LoginAuditLogResultDto"/> matching the provided criteria.</returns>
        Task<List<LoginAuditLogResultDto>> GetLoginAuditLogsAsync(LoginAuditLogQueryDto queryDto);

        /// <summary>
        /// Generates and stores a password reset token for the user identified by email.
        /// </summary>
        /// <param name="email">Email address of the user requesting the reset.</param>
        /// <returns><c>true</c> if the token was successfully generated and stored; otherwise <c>false</c>.</returns>
        Task<bool> GeneratePasswordResetTokenAsync(string email);

        /// <summary>
        /// Validates that a given reset token is valid and not expired for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user associated with the token.</param>
        /// <param name="token">The password reset token to validate.</param>
        /// <returns><c>true</c> if the token is valid and can be used; otherwise <c>false</c>.</returns>
        Task<bool> ValidatePasswordResetTokenAsync(string userId, string token);

        /// <summary>
        /// Resets the user's password using a valid reset token.
        /// </summary>
        /// <param name="dto">DTO containing the email, token, and new password.</param>
        /// <returns><c>true</c> if the password was successfully reset; otherwise <c>false</c>.</returns>
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);




    }
}
