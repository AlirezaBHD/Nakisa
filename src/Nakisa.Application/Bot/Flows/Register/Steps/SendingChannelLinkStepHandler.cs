using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Flows.Register.Steps;


public class SendingChannelLinkStepHandler : IRegisterStepHandler
{
    private readonly ITelegramClientService _client;

    public SendingChannelLinkStepHandler(ITelegramClientService client)
    {
        _client = client;
    }

    public RegisterStep Step => RegisterStep.SendingChannelLink;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var channelLink = update.Message!.Text;
        var isChannelFormatValid = channelLink.TryNormalizeTelegramChannelLink(out var cleanLink);
        var channelName = await _client.GetChannelInfoFromLinkAsync(cleanLink);
        if (isChannelFormatValid && channelName != null)
        {
            data.PersonChannelLink = cleanLink;
                
            switch (data.CaptionIdentifier)
            {
                case CaptionIdentifierType.Nickname:
                    data.CaptionIdentifier = CaptionIdentifierType.NicknameAndChannelName;
                    break;
                case CaptionIdentifierType.TelegramName:
                    data.CaptionIdentifier = CaptionIdentifierType.TelegramNameAndChannelName;
                    break;
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