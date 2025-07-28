using Nakisa.Application.DTOs.Category;
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
                InlineKeyboardButton.WithCallbackData($"-- {category.Name} --", $"category:{category.Id}")
            });

            var row = new List<InlineKeyboardButton>();
            int count = 0;

            foreach (var playlist in category.Playlists)
            {
                row.Add(InlineKeyboardButton.WithCallbackData($"{playlist.Emoji} {playlist.Name}", $"playlist:{playlist.Id}"));
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
}