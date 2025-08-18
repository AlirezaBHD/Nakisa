using Telegram.Bot.Types.ReplyMarkups;
using Types = Nakisa.Application.Bot.Constants.CallbackTypes;

namespace Nakisa.Application.Bot.Keyboards;

public static class MainKeyboard
{
    public static InlineKeyboardMarkup NewUserMainMenuButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("ثبت نام", Types.Register)
        ],
        [
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", Types.SubmitSong)
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", Types.ViewPlaylists)
        ]
    ]);
    
    
    public static InlineKeyboardMarkup OldUserMainMenuButton() => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("ویرایش پروفایل", Types.Register)
        ],
        [
            InlineKeyboardButton.WithCallbackData("ارسال آهنگ", Types.SubmitSong)
        ],
        [
            InlineKeyboardButton.WithCallbackData("دیدن پلیلیست ها", Types.ViewPlaylists)
        ]
    ]
        );
}
