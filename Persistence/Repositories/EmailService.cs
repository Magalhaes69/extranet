using System.Net;
using System.Net.Mail;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Persistence.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options)
        {
            this._emailSettings = options.Value;
        }
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(
                    _emailSettings.Username,
                    _emailSettings.Password),
                EnableSsl = _emailSettings.UseSSL
            };

            await smtp.SendMailAsync(message);
        }
    }
}
