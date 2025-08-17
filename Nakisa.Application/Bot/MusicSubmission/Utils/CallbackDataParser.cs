using Nakisa.Application.Bot.MusicSubmission.Constants;

namespace Nakisa.Application.Bot.MusicSubmission.Utils;

public record CallbackData(string Type, int Id, string? Action = null);

public static class CallbackDataParser
{
    public static CallbackData Parse(string raw)
    {
        var parts = raw.Split(":");
        return parts[0] switch
        {
            CallbackTypes.Category => new CallbackData(CallbackTypes.Category, int.Parse(parts[1])),
            CallbackTypes.Playlist => new CallbackData(CallbackTypes.Playlist, int.Parse(parts[1]), parts.ElementAtOrDefault(2)),
            _ => new CallbackData("unknown", 0)
        };
    }
}