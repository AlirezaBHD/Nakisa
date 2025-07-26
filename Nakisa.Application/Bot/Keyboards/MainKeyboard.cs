using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.Keyboards;

public static class MainKeyboard
{
    public static InlineKeyboardMarkup MainMenuButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("ثبت نام", "Register"),
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", "SubmitSong"),
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", "ViewPlaylists")
        ]
    ]);
}
