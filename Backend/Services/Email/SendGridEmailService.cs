using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Backend.Services.Interfaces.Email;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Backend.Services.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailService"/> that sends real emails using the SendGrid API.
    /// </summary>
    /// <remarks>
    /// Requires valid SendGrid API configuration via dependency injection and app settings.
    /// </remarks>

    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendGridEmailService"/> class.
        /// </summary>
        /// <param name="sendGridClient">The SendGrid client used to send emails.</param>
        /// <param name="logger">Logger instance for diagnostic and error logging.</param>
        /// <param name="config">Application configuration, used to retrieve sender information.</param>
        public SendGridEmailService(ISendGridClient sendGridClient, ILogger<SendGridEmailService> logger, IConfiguration config)
        {
            _sendGridClient = sendGridClient;
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Sends a two-factor authentication (2FA) token to the user's email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The generated 2FA token.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with <c>true</c> if the email was queued successfully.
        /// </returns>
        public async Task<bool> Send2FATokenAsync(string toEmail, string token)
        {
            var subject = "Your 2FA Verification Code";
            var body = $"Your 2FA code is: {token}. It expires in 10 minutes.";

            return await SendEmailAsync(toEmail, subject, body);
        }

        /// <summary>
        /// Sends a password reset token to the user's email address.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="token">The password reset token.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with <c>true</c> if the email was queued successfully.
        /// </returns>
        public async Task<bool> SendPasswordResetTokenAsync(string toEmail, string token)
        {
            var subject = "Reset Your Password";
            var body = $"Use the following token to reset your password: {token}. It expires in 30 minutes.";

            return await SendEmailAsync(toEmail, subject, body);
        }

        /// <summary>
        /// Internal helper method to send an email using the configured SendGrid client.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The plain text and HTML body of the email.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with <c>true</c> if the email was sent successfully.
        /// </returns>
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
