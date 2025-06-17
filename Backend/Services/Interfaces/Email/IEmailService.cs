namespace Backend.Services.Interfaces.Email
{
    /// <summary>
    /// Defines email-related services for sending authentication and password recovery messages.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a two-factor authentication (2FA) token to the user's email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The 2FA token to send.</param>
        /// <returns><c>true</c> if the email was successfully sent; otherwise <c>false</c>.</returns>
        Task<bool> Send2FATokenAsync(string toEmail, string token);

        /// <summary>
        /// Sends a password reset token to the user's email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The reset token to send.</param>
        /// <returns><c>true</c> if the email was successfully sent; otherwise <c>false</c>.</returns>
        Task<bool> SendPasswordResetTokenAsync(string toEmail, string token);
    }
}
