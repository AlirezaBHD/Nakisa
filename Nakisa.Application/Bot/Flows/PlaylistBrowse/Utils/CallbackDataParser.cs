using Type = Nakisa.Application.Bot.Core.Constants.PlaylistBrowseCallbackTypes;

namespace Nakisa.Application.Bot.Flows.PlaylistBrowse.Utils;

public record CallbackData(string Type, int Id);

public static class CallbackDataParser
{
    public static CallbackData Parse(string raw)
    {
        var parts = raw.Split(":");
        return parts[0] switch
        {
            Type.Category => new CallbackData(Type.Category, int.Parse(parts[1])),
            Type.Playlist => new CallbackData(Type.Playlist, int.Parse(parts[1])),
            _ => new CallbackData("unknown", 0)
        };
    }
}