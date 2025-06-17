namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a request to initiate the password reset process.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used when a user submits their email to receive a password reset link.
    /// </remarks>
    public class ForgotPasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the email address associated with the user account.
        /// </summary>
        /// <value>The user's email used for password reset communication.</value>
        public string Email { get; set; } = string.Empty;
    }
}
