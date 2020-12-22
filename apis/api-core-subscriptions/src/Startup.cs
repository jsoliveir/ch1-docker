using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Subscriptions.Configurations;
using Api.Core.Subscriptions.Databases;
using Api.Core.Subscriptions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using Prometheus;

namespace Api.Core.Subscriptions
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", false)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
               .AddEnvironmentVariables()
               .Build();

            SeqConfiguration = new SeqConfiguration();
            RabbitMQConfiguration = new RabbitMQConfiguration();
            Configuration.GetSection("RabbitMQ").Bind(RabbitMQConfiguration);
            Configuration.GetSection("Seq").Bind(SeqConfiguration);

            PrometheusRequestPathCounter = Metrics.CreateCounter(
                 "api_path_counter",
                 "Counts requests to the client subscrptions API endpoints",
                 new CounterConfiguration
                 {
                     LabelNames = new[] { "method", "endpoint" }
                 });
            PrometheusErrorCounter = Metrics.CreateCounter(
                   "api_error_counter",
                   "Counts API [400-500] Response Status Codes",
                   new CounterConfiguration
                   {
                       LabelNames = new[] { "status_code" }
                   });
        }

        public IConfigurationRoot Configuration { get; }
        public SeqConfiguration SeqConfiguration { get; }
        public RabbitMQConfiguration RabbitMQConfiguration { get; }
        public Counter PrometheusRequestPathCounter { get; }
        public Counter PrometheusErrorCounter { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddControllers();

            services.AddSwaggerGen(c =>
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

            services.AddSingleton(SeqConfiguration);
            services.AddSingleton(RabbitMQConfiguration);

            services.AddScoped<ICoreEmailService, CoreEmailService>();
            services.AddScoped<ISubscriptionsDbService, SubscriptionDbService>();

            services.AddDbContext<SubscriptionsDb>(
              options => options.UseInMemoryDatabase(databaseName: "Subscriptions"));

            services.AddRabbitMqProducingClientSingleton(RabbitMQConfiguration.Client);

            services.AddProductionExchange(
                RabbitMQConfiguration.ExchangeName,
                RabbitMQConfiguration.Exchange);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use((context, next) =>
            {
                PrometheusRequestPathCounter
                    .WithLabels(
                        context.Request.Method,
                        context.Request.Path)
                    .Inc();

                return next();
            });

            app.UseStatusCodePages(async context =>
            {
                PrometheusErrorCounter
                  .WithLabels(
                      context.HttpContext.Response.StatusCode.ToString())
                  .Inc();

            });

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
