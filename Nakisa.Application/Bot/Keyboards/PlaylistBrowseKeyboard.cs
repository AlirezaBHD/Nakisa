using Nakisa.Application.DTOs.Category;
using Nakisa.Application.DTOs.Playlist;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.Keyboards;

public static class PlaylistBrowseKeyboard
{
    public static InlineKeyboardMarkup CategoriesButton(IEnumerable<GetCategoryDto> categories)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();

        foreach (var category in categories)
        {
            keyboard.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData($"-- {category.Name} --", $"category:{category.Id}")
            });

            var row = new List<InlineKeyboardButton>();
            int count = 0;

            foreach (var playlist in category.Playlists)
            {
                row.Add(InlineKeyboardButton.WithCallbackData($"{playlist.Emoji} {playlist.Name}",
                    $"playlist:{playlist.Id}"));
                count++;

                if (count % 2 == 0)
                {
                    keyboard.Add(row);
                    row = new List<InlineKeyboardButton>();
                }
            }

            if (row.Any())
            {
                keyboard.Add(row);
            }
        }

        return new InlineKeyboardMarkup(keyboard);
    }
    
    public static InlineKeyboardMarkup CategoryPlaylistsButton(IEnumerable<MainPagePlaylistsDto> playlists)
    {
        var keyboard = new List<List<InlineKeyboardButton>>();

        var row = new List<InlineKeyboardButton>();
        int count = 0;

        foreach (var playlist in playlists)
        {
            row.Add(InlineKeyboardButton.WithCallbackData($"{playlist.Emoji} {playlist.Name}",
                $"playlist:{playlist.Id}:brows"));
            count++;

            if (count % 3 == 0)
            {
                keyboard.Add(row);
                row = new List<InlineKeyboardButton>();
            }
        }

        if (row.Any())
        {
            keyboard.Add(row);
        }

        return new InlineKeyboardMarkup(keyboard);
    }
    
    public static InlineKeyboardMarkup PlaylistsButton(List<BrowsePlaylistDto> playlists)
    {
        var mainPlaylist = playlists.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.Emoji));
        if (mainPlaylist != null)
        {
            playlists.Remove(mainPlaylist);
        }

        var keyboard = new List<List<InlineKeyboardButton>>();

        if (mainPlaylist != null)
        {
            keyboard.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithUrl(
                    $"- {mainPlaylist.Emoji} {mainPlaylist.Name} -",
                    mainPlaylist.ChannelInviteLink
                )
            });
        }

        for (int i = 0; i < playlists.Count; i += 2)
        {
            var row = new List<InlineKeyboardButton>();

            row.Add(InlineKeyboardButton.WithUrl(
                playlists[i].Name,
                playlists[i].ChannelInviteLink
            ));

            if (i + 1 < playlists.Count)
            {
                row.Add(InlineKeyboardButton.WithUrl(
                    playlists[i + 1].Name,
                    playlists[i+1].ChannelInviteLink
                ));
            }
            
            
            keyboard.Add(row);
        }
        var backRow = new List<InlineKeyboardButton>();
        backRow.Add(InlineKeyboardButton.WithCallbackData(
            "بازگشت",
            "back"
        ));
        
        keyboard.Add(backRow);
        return new InlineKeyboardMarkup(keyboard);
        
    }

}