using Bogus;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using XPRTZ.Webshop.ProductService.Consumers;
using XPRTZ.Webshop.ProductService.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.AddRabbitMQ("servicebus");

builder.Services.AddMassTransit(configure =>
    {
        configure.AddConsumer<ProductCatalogRequestConsumer>();
        configure.AddConsumer<ProductDetailsRequestConsumer>();

        configure.UsingRabbitMq((context, cfg) =>
        {
            var connection = context.GetRequiredService<IConnection>();

            cfg.Host(connection.Endpoint.HostName, "/", host =>
            {
                host.Username("guest");
                host.Password("guest");
            });

            cfg.ConfigureEndpoints(context);
        });

    });

builder.Services.AddSingleton(sp =>
    new Faker<Product>()
    .CustomInstantiator(f =>
    {
        var productName = f.Commerce.ProductName();

        return new Product(
            f.Commerce.Ean13(),
            productName,
            f.Commerce.ProductDescription(),
            Math.Round(f.Random.Decimal(min: 10, max: 100), 2),
            f.Rant.Reviews(productName, f.Random.Number(7)));
    })
    .Generate(50)
    .AsEnumerable()
);

await builder.Build().RunAsync();
