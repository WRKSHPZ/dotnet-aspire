
using System;
using Bogus;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XPRTZ.Webshop.ProductService.Consumers;
using XPRTZ.Webshop.ProductService.Models;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<ProductCatalogRequestConsumer>();
            configure.AddConsumer<ProductDetailsRequestConsumer>();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });

        });

        services.AddSingleton(sp =>
            new Faker<Product>()
            .CustomInstantiator(f =>
            {
                var productName = f.Commerce.ProductName();

                return new Product(
                    f.Commerce.Ean13(),
                    productName,
                    f.Commerce.ProductDescription(),
                    Math.Round(f.Random.Decimal(min: 10, max: 100), 2),
                    f.Random.Number(10),
                    f.Rant.Reviews(productName, f.Random.Number(7)));
            })
            .Generate(50)
            .AsEnumerable()
        );
    })
    .RunConsoleAsync();
