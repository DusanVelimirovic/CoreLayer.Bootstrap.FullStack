namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a request to initiate a password reset process.
    /// </summary>
    /// <remarks>
    /// This DTO is used when a user submits their email to receive a password reset link.
    /// </remarks>
    public class PasswordResetRequestDto
    {
        /// <summary>
        /// Gets or sets the email address associated with the user account.
        /// </summary>
        /// <value>The email address to which the password reset instructions will be sent.</value>
        public string Email { get; set; } = string.Empty;
    }
}
