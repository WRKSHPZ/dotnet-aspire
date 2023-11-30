using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using XPRTZ.Webshop.OrderService.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddRabbitMQ("servicebus");

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderPlacementConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {
        var connection = context.GetRequiredService<IConnection>();

        cfg.Host(new Uri(connection.Endpoint.ToString()), "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });

});

await builder.Build().RunAsync();
