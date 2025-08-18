using Nakisa.Application.Bot.Core.Enums;

namespace Nakisa.Application.Bot.Core.Session;

public class UserSession
{
    public long ChatId { get; set; }
    public UserFlow Flow { get; set; } = UserFlow.None;

    public object? FlowData { get; set; }

    public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
}