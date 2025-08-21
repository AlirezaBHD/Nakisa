namespace Nakisa.Contracts.Bot;

public class TelegramClientOptions
{
    public int ApiId { get; set; }
    public string? ApiHash { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SessionPath { get; set; }
}