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
    public class MailService : IMailService, IEmailReceiverObservable
    {
        private readonly ILogger<IMailService> _logger;
        private readonly IConfiguration _configurations;

        public MailService(
            IConfiguration configurations,
            IEmailReceiverObserver observers,
            ILogger<IMailService> logger)
        {
            _logger = logger;

            _configurations = configurations;

            observers.Attach(this);
        }

        public void OnEmailReceived(Email email) =>
            SendMail(email);

        public async Task SendMail(Email email)
        {
            try
            {
                var server = _configurations["Smtp:Server"];
                var port = int.Parse(_configurations["Smtp:Port"]);
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
                    _logger.LogInformation("mail sent: {mail}", email);
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
