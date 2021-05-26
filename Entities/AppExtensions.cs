using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Consul;

public static class AppExtensions
{
    public static string address { get; set; }
    public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            address = configuration.GetValue<string>("Consul:Host");
            consulConfig.Address = new Uri(address);
        }));
        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        if (!(app.Properties["server.Features"] is FeatureCollection features)) return app;

        var addresses = features.Get<IServerAddressesFeature>();

        Console.WriteLine($"address={address}");

        var uri = new Uri(address);
        var registration = new AgentServiceRegistration()
        {
            ID = $"MyService-{uri.Port}",
            Name = "MyService",
            Address = $"{uri.Host}",
            Port = uri.Port
        };

        logger.LogInformation("Registering with Consul");
        consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

        lifetime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("Unregistering from Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
        });

        return app;
    }
}