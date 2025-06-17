using Backend.Services.Interfaces.Email;

namespace Backend.Services.Email
{
    public class ConsoleEmailService : IEmailService
    {
        // Temporary Implementation
        public Task<bool> Send2FATokenAsync(string toEmail, string token)
        {
            Console.WriteLine($"[2FA EMAIL SIMULATION] To: {toEmail} | Token: {token}");
            return Task.FromResult(true);
        }

        public Task<bool> SendPasswordResetTokenAsync(string toEmail, string token)
        {
            Console.WriteLine($"[RESET EMAIL SIMULATION] To: {toEmail} | Token: {token}");
            return Task.FromResult(true);
        }


    }
}
