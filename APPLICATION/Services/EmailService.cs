using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.DTOs;
using Microsoft.Extensions.Options;

namespace APPLICATION.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync()
        {
            var client = new SmtpClient(_settings.SmtpHost)
            {
                Port = _settings.SmtpPort,
                Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.SmtpUser),
                Subject = "Someone Tries to log in",
                Body = "Someone tried to Log in",
                IsBodyHtml = false
            };
            mail.To.Add("chanuthnilushan@gmail.com");

            await client.SendMailAsync(mail);

        }

    }
}