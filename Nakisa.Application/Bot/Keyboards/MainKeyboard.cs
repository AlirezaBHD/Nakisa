using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.Keyboards;

public static class MainKeyboard
{
    public static InlineKeyboardMarkup NewUserMainMenuButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("ثبت نام", "Register")
        ],
        [
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", "SubmitSong")
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", "ViewPlaylists")
        ]
    ]);
    
    
    public static InlineKeyboardMarkup OldUserMainMenuButton() => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("ویرایش پروفایل", "Register")
        ],
        [
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", "SubmitSong")
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", "ViewPlaylists")
        ]
    ]
        );
}
