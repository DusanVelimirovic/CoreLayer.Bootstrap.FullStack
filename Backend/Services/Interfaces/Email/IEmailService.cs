namespace Backend.Services.Interfaces.Email
{
    public interface IEmailService
    {
        Task<bool> Send2FATokenAsync(string toEmail, string token);
        Task<bool> SendPasswordResetTokenAsync(string toEmail, string token);
    }
}
