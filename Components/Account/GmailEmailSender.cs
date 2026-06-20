using DiaspoDirect.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace DiaspoDirect.Components.Account
{
    public class GmailEmailSender : IEmailSender<ApplicationUser>, IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<GmailEmailSender> _logger;

        public GmailEmailSender(IConfiguration config, ILogger<GmailEmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            SendEmailAsync(email, "Confirm your DiaspoDirect account",
                $"Welcome to DiaspoDirect! Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            SendEmailAsync(email, "Reset your DiaspoDirect password",
                $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            SendEmailAsync(email, "Reset your DiaspoDirect password",
                $"Your password reset code is: <strong>{resetCode}</strong>");

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var fromAddress = _config["Email:From"] ?? "diaspodirect88@gmail.com";
            var appPassword = _config["Email:AppPassword"]
                ?? throw new InvalidOperationException("Email:AppPassword is not configured.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DiaspoDirect", fromAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromAddress, appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent to {Email} — subject: {Subject}", toEmail, subject);
        }
    }
}
