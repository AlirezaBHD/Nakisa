using Nakisa.Application.Bot.PlaylistBrowse.Constants;

namespace Nakisa.Application.Bot.PlaylistBrowse.Utils;

public record CallbackData(string Type, int Id);

public static class CallbackDataParser
{
    public static CallbackData Parse(string raw)
    {
        var parts = raw.Split(":");
        return parts[0] switch
        {
            CallbackTypes.Category => new CallbackData(CallbackTypes.Category, int.Parse(parts[1])),
            CallbackTypes.Playlist => new CallbackData(CallbackTypes.Playlist, int.Parse(parts[1])),
            _ => new CallbackData("unknown", 0)
        };
    }
}