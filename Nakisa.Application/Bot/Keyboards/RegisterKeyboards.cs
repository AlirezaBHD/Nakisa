using Telegram.Bot.Types.ReplyMarkups;
using Type = Nakisa.Application.Bot.Register.Constants.CallbackTypes;

namespace Nakisa.Application.Bot.Keyboards;

public static class RegisterKeyboards
{
    public static InlineKeyboardMarkup ChooseIdentityButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("با اسم تلگرامت", Type.TelegramName),
            InlineKeyboardButton.WithCallbackData("با اسم مستعار", Type.Nickname),
            InlineKeyboardButton.WithCallbackData("ناشناس", Type.Unknown)
        ]
    ]);

    public static InlineKeyboardMarkup LinkTypeButton() => new([
        [InlineKeyboardButton.WithCallbackData("وصل بشه به یوزرنیم تلگرامت", Type.Username)],
        [InlineKeyboardButton.WithCallbackData("وصل بشه به لینک کانالت", Type.ChannelLink)],
        [InlineKeyboardButton.WithCallbackData("نه، نمی‌خوام", Type.None)]
    ]);

    public static InlineKeyboardMarkup ChannelLinkPrefixButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("بله", Type.Yes),
            InlineKeyboardButton.WithCallbackData("خیر", Type.No)
        ]
    ]);
}
