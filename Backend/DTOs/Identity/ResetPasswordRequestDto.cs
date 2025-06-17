namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to reset a user's password.
    /// </summary>
    /// <remarks>
    /// This DTO is used during the password reset process after the user has received a reset token via email.
    /// </remarks>
    public class ResetPasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the email address of the user requesting the password reset.
        /// </summary>
        /// <value>The user's email address.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token received via email for verifying the password reset request.
        /// </summary>
        /// <value>The password reset verification token.</value>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the new password that will replace the old one.
        /// </summary>
        /// <value>The new password to be set for the user account.</value>
        public string NewPassword { get; set; } = string.Empty;
    }
}
