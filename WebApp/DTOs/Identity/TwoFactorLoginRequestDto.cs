namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a request to initiate two-factor authentication (2FA) login for a specific user.
    /// </summary>
    /// <remarks>
    /// This DTO is used during the second step of authentication after the primary credentials are validated.
    /// </remarks>
    public class TwoFactorLoginRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user attempting 2FA login.
        /// </summary>
        /// <value>A string that identifies the user (e.g., a GUID or user ID).</value>
        public string UserId { get; set; } = string.Empty;
    }
}
