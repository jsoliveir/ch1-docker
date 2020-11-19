using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Api.Client.Subscriptions.Authentication;
using Api.Client.Subscriptions.Middlewares;
using Api.Core.Subscriptions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api.Client.Subscriptions
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
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"documentation.xml";
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1",  new OpenApiInfo
                {
                    Title = "Subscriptions API - V1",
                    Version = "v1",
                    Description = "Use the following link " + 
                        "to check incomming emails after creating a subscription:<br/>" +
                        "http://localhost:8080/private/smtp/",
                    Contact = new OpenApiContact
                    {
                        Name = "José Oliveira",
                        Email = "jsoliveira.dev@outlook.pt"
                    },
                });

                c.AddSecurityDefinition("Bearer token authorization", new OpenApiSecurityScheme
                {
                    Description = "This API is not validating any kind of tokens.<br/>" +
                        "The implemented auth flow is for demo porposes only.<br/> " + 
                        "Just type any string in the authorization box.",
                    Scheme = "Bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer token authorization"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            })
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationOptions.DefaultScheme, (o) => { });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder
                .SetMinimumLevel(Enum.Parse<LogLevel>(Configuration["Seq:MinimumLevel"]))
                .AddSeq(Configuration["Seq:ServerUrl"]);
            });

            services.AddHttpClient<ICoreSubscriptionsService, CoreSubscriptionsService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Api:CoreSubscriptions:BaseAddress"]);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c=>
            {
                c.PreSerializeFilters.Add((document, request) =>
                {
                    if (request.Headers.TryGetValue("X-Forwarded-Path",out var path))
                    {
                        document.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer()
                            { 
                                Url = $"{path}" 
                            }
                        };
                    }
                });
            });

            app.UseMiddleware<HttpExceptionMiddleware>();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", this.GetType().Namespace);
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
