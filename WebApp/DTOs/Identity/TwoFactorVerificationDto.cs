namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to verify a user's two-factor authentication (2FA) code.
    /// </summary>
    /// <remarks>
    /// This DTO is used when submitting the 2FA token to complete the login process.
    /// </remarks>
    public class TwoFactorVerificationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user performing 2FA verification.
        /// </summary>
        /// <value>A string that uniquely identifies the user.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the two-factor authentication token provided by the user.
        /// </summary>
        /// <value>The one-time verification code (e.g., from an authenticator app or SMS).</value>
        public string Token { get; set; } = string.Empty;
    }
}
