using Backend.Services.Interfaces.Email;

namespace Backend.Services.Email
{
    /// <summary>
    /// Simulated email service that logs email content to the console instead of sending real emails.
    /// </summary>
    /// <remarks>
    /// Useful for local development or testing without configuring an actual SMTP provider.
    /// </remarks>
    public class ConsoleEmailService : IEmailService
    {
        /// <summary>
        /// Simulates sending a 2FA token to the specified email by logging the token to the console.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The generated 2FA token.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing <c>true</c> to indicate success.
        /// </returns>
        public Task<bool> Send2FATokenAsync(string toEmail, string token)
        {
            Console.WriteLine($"[2FA EMAIL SIMULATION] To: {toEmail} | Token: {token}");
            return Task.FromResult(true);
        }

        /// <summary>
        /// Simulates sending a password reset token to the specified email by logging the token to the console.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The generated password reset token.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing <c>true</c> to indicate success.
        /// </returns>
        public Task<bool> SendPasswordResetTokenAsync(string toEmail, string token)
        {
            Console.WriteLine($"[RESET EMAIL SIMULATION] To: {toEmail} | Token: {token}");
            return Task.FromResult(true);
        }


    }
}
