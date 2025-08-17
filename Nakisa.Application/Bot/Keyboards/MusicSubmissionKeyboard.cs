using Types = Nakisa.Application.Bot.MusicSubmission.Constants.CallbackTypes;
using Nakisa.Application.DTOs.Category;
using Nakisa.Application.DTOs.Playlist;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.Keyboards;

public static class MusicSubmissionKeyboard
{
    public static InlineKeyboardMarkup CategoriesButton(IEnumerable<GetCategoryDto> categories)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();

        foreach (var category in categories)
        {
            keyboard.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(
                    $"-- {category.Name} --",
                    $"{Types.Category}:{category.Id}")
            });

            var playlistRows = BuildKeyboard(
                category.Playlists,
                p => BuildButton($"{p.Emoji} {p.Name}", p.Id, Types.PlaylistActions.Browse),
                rowSize: 2
            );

            keyboard.AddRange(playlistRows);
        }

        return new InlineKeyboardMarkup(keyboard);
    }

    public static InlineKeyboardMarkup CategoryPlaylistsButton(IEnumerable<MainPagePlaylistsDto> playlists)
    {
        var keyboard = BuildKeyboard(
            playlists,
            p => BuildButton($"{p.Emoji} {p.Name}", p.Id, Types.PlaylistActions.Browse),
            rowSize: 3
        );

        return new InlineKeyboardMarkup(keyboard);
    }

    public static InlineKeyboardMarkup PlaylistsButton(List<MainPagePlaylistsDto> playlists)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();

        var mainPlaylist = playlists.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Emoji));
        if (mainPlaylist != null)
        {
            playlists.Remove(mainPlaylist);
            keyboard.Add(new List<InlineKeyboardButton>
            {
                BuildButton($"- {mainPlaylist.Emoji} {mainPlaylist.Name} -",
                            mainPlaylist.Id,
                            Types.PlaylistActions.Submit)
            });
        }

        var otherRows = BuildKeyboard(
            playlists,
            p => BuildButton(p.Name, p.Id, Types.PlaylistActions.Submit),
            rowSize: 2
        );

        keyboard.AddRange(otherRows);

        return new InlineKeyboardMarkup(keyboard);
    }

    private static InlineKeyboardButton BuildButton(string text, int id, string action)
    {
        return InlineKeyboardButton.WithCallbackData(
            text,
            $"{Types.Playlist}:{id}:{action}"
        );
    }

    private static List<List<InlineKeyboardButton>> BuildKeyboard<T>(
        IEnumerable<T> items,
        Func<T, InlineKeyboardButton> buttonFactory,
        int rowSize)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();
        var row = new List<InlineKeyboardButton>();

        foreach (var item in items)
        {
            row.Add(buttonFactory(item));
            if (row.Count == rowSize)
            {
                keyboard.Add(row);
                row = new List<InlineKeyboardButton>();
            }
        }

        if (row.Any())
            keyboard.Add(row);

        return keyboard;
    }
}