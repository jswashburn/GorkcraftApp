namespace GCServer;

public class GCServerSettings
{
    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;

    public string[]? AllowedEmails { get; set; }
}
