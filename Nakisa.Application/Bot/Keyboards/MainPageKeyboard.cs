using Telegram.Bot.Types.ReplyMarkups;
using Type = Nakisa.Application.Bot.Core.Constants.MainPageCallbackTypes;

namespace Nakisa.Application.Bot.Keyboards;

public static class MainPageKeyboard
{
    public static InlineKeyboardMarkup NewUserMainMenuButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("ثبت نام", Type.Register)
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", Type.ViewPlaylists)
        ]
    ]);
    
    
    public static InlineKeyboardMarkup OldUserMainMenuButton() => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("ویرایش پروفایل", Type.Register)
        ],
        [
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", Type.SubmitSong)
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", Type.ViewPlaylists)
        ]
    ]
        );
}
