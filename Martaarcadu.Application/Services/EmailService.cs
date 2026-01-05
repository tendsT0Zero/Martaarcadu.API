using MailKit.Net.Smtp;
using MailKit.Security;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Martaarcadu.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            // Sender and Receiver
            emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            // Content (HTML supported)
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // Send Email using MailKit
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);

                    // Authenticate
                    await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);

                    // Send
                    await client.SendAsync(emailMessage);
                }
                catch (Exception ex)
                {
                    
                    throw new Exception($"Failed to send email: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}