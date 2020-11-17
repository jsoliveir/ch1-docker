using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Mail.Configurations;
using Api.Core.Mail.Observers;
using Api.Core.Mail.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;

namespace Api.Core.Mail
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public SmtpConfiguration SmtpConfiguration { get; }
        public SeqConfiguration SeqConfiguration { get; }

        public RabbitMQConfiguration RabbitConfiguration { get; }

        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json",false)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json",true)
               .AddEnvironmentVariables()
               .Build();

            SmtpConfiguration = new SmtpConfiguration();
            SeqConfiguration = new SeqConfiguration();
            RabbitConfiguration = new RabbitMQConfiguration();

            Configuration.GetSection("RabbitMQ").Bind(RabbitConfiguration);
            Configuration.GetSection("Seq").Bind(SeqConfiguration);
            Configuration.GetSection("Smtp").Bind(SmtpConfiguration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c=>
            {
                var xmlFile = $"documentation.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                .SetMinimumLevel(SeqConfiguration.MinimumLevel)
                .AddSeq(SeqConfiguration.ServerUrl);
            });

            services.AddSingleton(SmtpConfiguration);
            services.AddSingleton(SeqConfiguration);
            services.AddSingleton(RabbitConfiguration);

            services.AddSingleton<IEmailService, EmailService>();

            services.AddRabbitMqConsumingClientSingleton(
                RabbitConfiguration.Client);

            services.AddConsumptionExchange(
               exchangeName: RabbitConfiguration.ExchangeName,
               options: RabbitConfiguration.Exchange);

            services.AddAsyncMessageHandlerSingleton<MessageHandler>(
                RabbitConfiguration.Exchange.Queues.SelectMany(q=>q.RoutingKeys));

            services.AddHostedService<MessagingService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", this.GetType().Namespace);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
