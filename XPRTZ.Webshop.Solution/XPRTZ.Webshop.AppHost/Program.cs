var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQContainer("servicebus");

var api = builder
    .AddProject<Projects.XPRTZ_Webshop_API>("xprtz.webshop.api")
    .WithReference(rabbitMq);

builder
    .AddProject<Projects.XPRTZ_Webshop_Site>("xprtz.webshop.site")
    .WithReference(api);

builder
    .AddProject<Projects.XPRTZ_Webshop_OrderService>("xprtz.webshop.orderservice")
    .WithReference(rabbitMq);

builder
    .AddProject<Projects.XPRTZ_Webshop_ProductService>("xprtz.webshop.productservice")
    .WithReference(rabbitMq);

builder
    .AddProject<Projects.XPRTZ_Webshop_StockService>("xprtz.webshop.stockservice")
    .WithReference(rabbitMq);

builder.Build().Run();
