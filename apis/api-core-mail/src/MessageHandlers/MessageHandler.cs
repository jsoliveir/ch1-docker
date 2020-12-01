using Api.Core.Mail.Models;
using Api.Core.Mail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Core.Mail.Observers
{
    public class MessageHandler : IAsyncMessageHandler
    {
        private readonly IEmailService _service;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(IEmailService service,ILogger<MessageHandler> logger)
        {
            _service = service;
            _logger = logger;
        }
        public Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute)
        {
            try
            {
                var message = eventArgs.GetMessage();
                _logger.LogTrace($"mail.event arrived from [{matchingRoute}] : {message}");
                var email = JsonSerializer.Deserialize<Email>(message);
                return _service.SendMail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return Task.CompletedTask;
        }

    }
}
