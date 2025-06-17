using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Backend.Services.Interfaces.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Backend.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly IConfiguration _config;

        public SendGridEmailService(ISendGridClient sendGridClient, ILogger<SendGridEmailService> logger, IConfiguration config)
        {
            _sendGridClient = sendGridClient;
            _logger = logger;
            _config = config;
        }

        public async Task<bool> Send2FATokenAsync(string toEmail, string token)
        {
            var subject = "Your 2FA Verification Code";
            var body = $"Your 2FA code is: {token}. It expires in 10 minutes.";

            return await SendEmailAsync(toEmail, subject, body);
        }

        public async Task<bool> SendPasswordResetTokenAsync(string toEmail, string token)
        {
            var subject = "Reset Your Password";
            var body = $"Use the following token to reset your password: {token}. It expires in 30 minutes.";

            return await SendEmailAsync(toEmail, subject, body);
        }

        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var senderEmail = _config["SendGrid:SenderEmail"];
            var senderName = _config["SendGrid:SenderName"];

            if (string.IsNullOrWhiteSpace(senderEmail))
            {
                _logger.LogError("[SendGrid] Sender email configuration missing.");
                return false;
            }

            var msg = new SendGridMessage
            {
                From = new EmailAddress(senderEmail, senderName ?? "Trlić Procurement"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };

            msg.AddTo(new EmailAddress(toEmail));

            _logger.LogInformation("[SendGrid] Sending email to {Email} with subject '{Subject}'", toEmail, subject);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("[SendGrid] ✅ Email successfully queued to {Email}.", toEmail);
                return true;
            }
            else
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("[SendGrid] ❌ Failed to send email to {Email}. Status: {StatusCode}. Error: {ErrorBody}", toEmail, response.StatusCode, errorBody);
                return false;
            }
        }
    }
}
