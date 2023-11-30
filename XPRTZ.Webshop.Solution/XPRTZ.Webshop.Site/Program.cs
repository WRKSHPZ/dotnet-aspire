using XPRTZ.Webshop.Site.Components;
using MudBlazor.Services;
using XPRTZ.Webshop.Site.Settings;
using Microsoft.Extensions.Options;
using XPRTZ.Webshop.Site.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddHttpClient(Options.DefaultName, (sp, client) =>
{
    client.BaseAddress = new Uri("http://xprtz.webshop.api");
});

builder.Services.AddScoped<Basket>();

builder.Services
    .AddOptions<RabbitMQSettings>()
    .ValidateDataAnnotations()
    .ValidateOnStart()
    .Bind(builder.Configuration.GetSection(nameof(RabbitMQSettings)));

builder.Services
    .AddOptions<APISettings>()
    .ValidateDataAnnotations()
    .ValidateOnStart()
    .Bind(builder.Configuration.GetSection(nameof(APISettings)));

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
