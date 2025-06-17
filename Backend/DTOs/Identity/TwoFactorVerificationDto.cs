namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a request to verify a user's two-factor authentication (2FA) token.
    /// </summary>
    /// <remarks>
    /// This DTO is used when a user submits their 2FA token for verification during the login process.
    /// </remarks>
    public class TwoFactorVerificationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The ID of the user attempting 2FA verification.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the two-factor authentication token provided by the user.
        /// </summary>
        /// <value>The 2FA token submitted for verification.</value>
        public string Token { get; set; } = string.Empty;
    }
}
