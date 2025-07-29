using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;


public class SendingChannelLinkStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.SendingChannelLink;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var channelLink = update.Message!.Text;

        if (channelLink.TryNormalizeTelegramChannelLink(out var cleanLink))
        {
            data.PersonChannelLink = cleanLink;
                
            if (data.CaptionIdentifier == CaptionIdentifierType.Nickname)
            {
                data.CaptionIdentifier = CaptionIdentifierType.NicknameAndChannelName;
            }
            else if (data.CaptionIdentifier == CaptionIdentifierType.TelegramName)
            {
                data.CaptionIdentifier = CaptionIdentifierType.TelegramNameAndChannelName;
            }

            data.Step = RegisterStep.ChannelPrefix;
                
            await bot.SendMessage(
                chatId: chatId,
                text: "میخوای چنلت زیر پست ها نشون داده بشه؟",
                replyMarkup: RegisterKeyboards.ChannelLinkPrefixButton(),
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "لینک ارسال شده اشتباه است \nفرمت صحیح:\nhttps://t.me/+WZHMwFGZM4NlMTZk\nt.me/+WZHMwFGZM4NlMTZk\n@TheOsservatore\nt.me/TheOsservatore\nhttps://t.me/TheOsservatore",
                cancellationToken: ct);
        }
    }
}