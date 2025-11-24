using Types = Nakisa.Application.Bot.Core.Constants.MusicSubmissionCallbackTypes;

namespace Nakisa.Application.Bot.Flows.MusicSubmission.Utils;

public record CallbackData(string Type, int Id, string? Action = null);

public static class CallbackDataParser
{
    public static CallbackData Parse(string raw)
    {
        var parts = raw.Split(":");
        return parts[0] switch
        {
            Types.Category => new CallbackData(Types.Category, int.Parse(parts[1])),
            Types.Playlist => new CallbackData(Types.Playlist, int.Parse(parts[1]), parts.ElementAtOrDefault(2)),
            _ => new CallbackData("unknown", 0)
        };
    }
}