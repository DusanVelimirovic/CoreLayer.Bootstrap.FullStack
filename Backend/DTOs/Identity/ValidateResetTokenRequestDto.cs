namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to validate a password reset token.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used during the password reset flow to confirm that the reset token is valid for a specific user.
    /// </remarks>
    public class ValidateResetTokenRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user associated with the password reset request.
        /// </summary>
        /// <value>The user's ID.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password reset token that needs to be validated.
        /// </summary>
        /// <value>The reset token.</value>
        public string Token { get; set; } = string.Empty;
    }
}
