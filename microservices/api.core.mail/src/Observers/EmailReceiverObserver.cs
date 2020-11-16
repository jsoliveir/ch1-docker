using Api.Core.Mail.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
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
    public class EmailReceiverObserver : IEmailReceiverObserver, IDisposable
    {
        private readonly ILogger<IEmailReceiverObserver> _logger;
        private readonly List<IEmailReceiverObservable> _observers;
        private readonly IConfiguration _configurations;
        private readonly ConnectionFactory _connectionfactory;
        private readonly Timer _ensureConnectionTimer;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;

        public EmailReceiverObserver(
            IConfiguration configurations,
            ILogger<IEmailReceiverObserver> logger)
        {
            _logger = logger;

            _observers = new List<IEmailReceiverObservable>();

            _configurations = configurations;

            _connectionfactory = new ConnectionFactory()
            {
                HostName = _configurations["RabbitMQ:Server"],
                Port = int.Parse(_configurations["RabbitMQ:Port"])
            };

            _ensureConnectionTimer = new Timer(EnsureMQConnection, null, 1000, 10000);
        }

        public void EnsureMQConnection(object state)
        {
            try
            {
                if (_connection?.IsOpen ?? false)
                    return;

                _connection = _connectionfactory.CreateConnection();
                _channel = _connection.CreateModel();
                _consumer = new EventingBasicConsumer(_channel);
                _consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var email = JsonSerializer.Deserialize<Email>(message,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    Notify(email);
                };

                _channel.QueueDeclare(
                    queue: _configurations["RabbitMQ:Queue"],
                    durable: true,
                    autoDelete: false,
                    exclusive: false);

                _channel.BasicConsume(
                    queue: _configurations["RabbitMQ:Queue"],
                    consumer: _consumer);

                _logger.LogInformation("Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }

        public void Attach(IEmailReceiverObservable observer)
        {
            _observers.Add(observer);
        }
        public void Dettach(IEmailReceiverObservable observer)
        {
            _observers.Remove(observer);
        }
        public void Notify(Email message)
        {
            foreach (var observer in _observers)
                observer.OnEmailReceived(message);
        }


    }
}
