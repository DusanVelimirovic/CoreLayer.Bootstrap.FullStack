namespace WebApp.DTOs.Identity
{

    /// <summary>
    /// Represents the data required to reset a user's password.
    /// </summary>
    /// <remarks>
    /// This DTO is used in the password reset process after the user has received a valid reset token.
    /// </remarks>
    public class ResetPasswordRequestDto
    {
        /// <summary>
        /// Gets or sets the email address of the user requesting a password reset.
        /// </summary>
        /// <value>The user's registered email address.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password reset token sent to the user's email.
        /// </summary>
        /// <value>A token used to authorize the password reset request.</value>
        public string Token { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the new password to be set for the user.
        /// </summary>
        /// <value>The new password defined by the user.</value>
        public string NewPassword { get; set; } = string.Empty;
    }
}
