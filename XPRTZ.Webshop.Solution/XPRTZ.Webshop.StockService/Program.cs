using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.StockService.Consumers;
using XPRTZ.Webshop.StockService.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.AddRabbitMQ("servicebus");

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<ProductSoldConsumer>();
    configure.AddConsumer<ProductStockRequestConsumer>();

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

builder.Services.AddSingleton(sp =>
{
    var requestClient = sp.CreateScope().ServiceProvider.GetRequiredService<IRequestClient<ProductCatalogRequest>>();

    var products = requestClient.GetResponse<ProductCatalogResponse>(new ProductCatalogRequest()).GetAwaiter().GetResult().Message.Products;

    return products.Select(p => new ProductStock(p.EAN, Random.Shared.Next(0, 10)));
});

await builder.Build().RunAsync();
