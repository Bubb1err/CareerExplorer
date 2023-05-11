using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace CareerExplorer.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(MailboxAddress.Parse(_config.GetSection("Email:Sender").Value));
                mailMessage.Subject = subject;
                mailMessage.To.Add(MailboxAddress.Parse(email));
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = htmlMessage
                };
                using (SmtpClient client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config.GetSection("Email:Sender").Value, 
                        _config.GetSection("Email:Password").Value);
                    await client.SendAsync(mailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.ToString());
            }

        }
    }
}
