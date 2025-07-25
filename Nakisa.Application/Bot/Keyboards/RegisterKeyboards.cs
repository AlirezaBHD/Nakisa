using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.Keyboards;

public static class RegisterKeyboards
{
    public static InlineKeyboardMarkup ChooseIdentityButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("با اسم تلگرامت", "TelegramName"),
            InlineKeyboardButton.WithCallbackData("با اسم مستعار", "Nickname"),
            InlineKeyboardButton.WithCallbackData("ناشناس", "Unknown")
        ]
    ]);

    public static InlineKeyboardMarkup LinkTypeButton() => new([
        [InlineKeyboardButton.WithCallbackData("وصل بشه به یوزرنیم تلگرامت", "Username")],
        [InlineKeyboardButton.WithCallbackData("وصل بشه به لینک کانالت", "ChannelLink")],
        [InlineKeyboardButton.WithCallbackData("نه، نمی‌خوام", "None")]
    ]);

    public static InlineKeyboardMarkup ChannelLinkPrefixButton() => new([
        [
            InlineKeyboardButton.WithCallbackData("بله", "Yes"),
            InlineKeyboardButton.WithCallbackData("خیر", "No")
        ]
    ]);
}
