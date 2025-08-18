using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Type = Nakisa.Application.Bot.Core.Constants.RegisterCallbackTypes;

namespace Nakisa.Application.Bot.Flows.Register.Steps;

public class ChooseLinkTypeStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.ChooseLinkType;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callBack = update.GetCallbackData();
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();

        switch (callBack)
        {
            case Type.Username:
                await HandleUsernameOptionAsync(data, chatId, messageId, bot, update, ct);
                break;

            case Type.ChannelLink:
                await HandleChannelLinkOptionAsync(data, chatId, messageId, bot, ct);
                break;

            case Type.None:
                await HandleNoneOptionAsync(data, chatId, messageId, bot, ct);
                break;
        }
    }

    private async Task HandleUsernameOptionAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot,
        Update update, CancellationToken ct)
    {
        var username = update.Message?.From?.Username
                       ?? update.CallbackQuery?.From?.Username;
        if (username == null)
        {
            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text:
                "آیدیتو هاید کردی نمیتونم بهش دسترسی داشته باشم.\nدرستش کن یا یه گزینه دیگه رو انتخواب کن",
                replyMarkup: RegisterKeyboards.LinkTypeButton(),
                cancellationToken: ct);
            return;
        }

        switch (data.CaptionIdentifier)
        {
            case CaptionIdentifierType.Nickname:
                data.CaptionIdentifier = CaptionIdentifierType.NicknameAndUsername;
                break;
            case CaptionIdentifierType.TelegramName:
                data.CaptionIdentifier = CaptionIdentifierType.TelegramNameAndUsername;
                break;
        }

        data.Username = username; //user might change that

        data.Step = RegisterStep.ChannelPrefix;

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "میخوای چنلت زیر پست ها نشون داده بشه؟",
            replyMarkup: RegisterKeyboards.ChannelLinkPrefixButton(),
            cancellationToken: ct);
    }

    private async Task HandleChannelLinkOptionAsync(RegisterDto data, long chatId, int messageId,
        ITelegramBotClient bot, CancellationToken ct)
    {
        data.Step = RegisterStep.SendingChannelLink;

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "لینک چنلتو بفرست",
            cancellationToken: ct);
    }

    private async Task HandleNoneOptionAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot,
        CancellationToken ct)
    {
        data.Step = RegisterStep.ChannelPrefix;

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "میخوای چنلت زیر پست ها نشون داده بشه؟",
            replyMarkup: RegisterKeyboards.ChannelLinkPrefixButton(),
            cancellationToken: ct);
    }
}