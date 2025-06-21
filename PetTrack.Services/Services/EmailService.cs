using Microsoft.Extensions.Options;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Config;
using System.Net;
using System.Net.Mail;

namespace PetTrack.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.UserName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
