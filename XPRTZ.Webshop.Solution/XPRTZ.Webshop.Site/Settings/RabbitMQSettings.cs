namespace XPRTZ.Webshop.Site.Settings;

public record RabbitMQSettings
{
    public string Host { get; init; } = string.Empty;

    public string User { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
