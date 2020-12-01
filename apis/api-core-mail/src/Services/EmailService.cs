using Api.Core.Mail.Configurations;
using Api.Core.Mail.Models;
using Api.Core.Mail.Observers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Mail.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<IEmailService> _logger;
        private readonly SmtpConfiguration _configurations;

        public EmailService(
            ILogger<IEmailService> logger,
            SmtpConfiguration configurations)
        {
            _logger = logger;
            _configurations = configurations;
        }

        public async Task SendMail(Email email)
        {
            try
            {
                var server = _configurations.Server;
                var port = _configurations.Port;
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(email.From),
                    Subject = email.Subject,
                    Body = email.Body
                };
                mailMessage.To.Add(new MailAddress(email.To));
                using (SmtpClient smtp = new SmtpClient(server, port))
                {
                    await smtp.SendMailAsync(mailMessage);
                    _logger.LogWarning("mail sent! {mail}", email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), email);
                throw;
            }
        }
    }
}
