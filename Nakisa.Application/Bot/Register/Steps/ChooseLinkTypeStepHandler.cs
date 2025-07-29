using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChooseLinkTypeStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.ChooseLinkType;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callBack = update.CallbackQuery!.Data;
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        
        switch (callBack)
        {
            case "Username":
                var username = update.Message?.From?.Username
                               ?? update.CallbackQuery?.From?.Username;
                if (username == null)
                {
                    await bot.EditMessageText(
                        chatId: chatId,
                        messageId: messageId,
                        text: "آیدیتو هاید کردی نمیتونم بهش دسترسی داشته باشم.\nدرستش کن یا یه گزینه دیگه رو انتخواب کن",
                        replyMarkup: RegisterKeyboards.LinkTypeButton(),
                        cancellationToken: ct);
                    break;
                }

                if (data.CaptionIdentifier == CaptionIdentifierType.Nickname)
                {
                    data.CaptionIdentifier = CaptionIdentifierType.NicknameAndUsername;
                }
                else if (data.CaptionIdentifier == CaptionIdentifierType.TelegramName)
                {
                    data.CaptionIdentifier = CaptionIdentifierType.TelegramNameAndUsername;
                }

                data.Username = username; //user might change that
                
                data.Step = RegisterStep.ChannelPrefix;
                
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "میخوای چنلت زیر پست ها نشون داده بشه؟",
                    replyMarkup: RegisterKeyboards.ChannelLinkPrefixButton(),
                    cancellationToken: ct);
                
                break;

            case "ChannelLink":

                data.Step = RegisterStep.SendingChannelLink;

                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "لینک چنلتو بفرست",
                    cancellationToken: ct);

                break;

            case "None":
                data.Step = RegisterStep.ChannelPrefix;

                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "میخوای چنلت زیر پست ها نشون داده بشه؟",
                    replyMarkup: RegisterKeyboards.ChannelLinkPrefixButton(),
                    cancellationToken: ct);

                break;
        }
    }
}