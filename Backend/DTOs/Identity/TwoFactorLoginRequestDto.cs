namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a request to complete login after successful two-factor authentication verification.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used after a user successfully verifies their 2FA token and is ready to be signed in.
    /// </remarks>
    public class TwoFactorLoginRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The ID of the user who has completed 2FA verification.</value>
        public string UserId { get; set; } = string.Empty;
    }
}
