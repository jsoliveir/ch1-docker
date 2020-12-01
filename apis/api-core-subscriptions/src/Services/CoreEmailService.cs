using Api.Core.Subscriptions.Configurations;
using Api.Core.Subscriptions.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Services
{
    public class CoreEmailService : ICoreEmailService
    {
        private IProducingService _broker;
        private ILogger<ICoreEmailService> _logger;
        private RabbitMQConfiguration _configuration;

        public CoreEmailService(
            IProducingService broker,
            ILogger<ICoreEmailService> logger,
            RabbitMQConfiguration configuration)
        {
            _broker = broker;
            _logger = logger;
            _configuration = configuration;
        }

        public Task Send(Email mail)
        {
            var routingKey = _configuration
                .Exchange.Queues.SelectMany(q => q.RoutingKeys);

            var exchangeName = _configuration.ExchangeName;

            _logger.LogWarning("sending mail.event ...");

            return _broker.SendAsync(
                @object: mail,
                exchangeName: exchangeName,
                routingKey: routingKey.LastOrDefault());
        }

    }
}
