using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;

namespace Api.Core.Mail.Configurations
{
    public class RabbitMQConfiguration
    {
        public string ExchangeName { get; set; }

        public RabbitMqClientOptions Client { get; set; }

        public RabbitMqExchangeOptions Exchange { get; set; }
    }

}
