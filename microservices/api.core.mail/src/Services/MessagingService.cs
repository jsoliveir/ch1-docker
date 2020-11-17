using Api.Core.Mail.Configurations;
using Api.Core.Mail.Models;
using Api.Core.Mail.Observers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Core.Mail.Services
{
    public class MessagingService : IHostedService
    {
        private readonly ILogger<IEmailService> _logger;
        private readonly IConsumingService _service;

        public MessagingService(ILogger<IEmailService> logger,
            IConsumingService service)
        {
            _logger = logger;
            _service = service;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Messaging service Started");
            return Task.Run(_service.StartConsuming);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Messaging service Stopped");
            return Task.Run(_service.StopConsuming);
        }
    }
}
