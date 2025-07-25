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
        var channelLink = update.Message!.Text;
        //validate channel link
        var chatId = update.GetChatId();

        data.PersonChannelLink = channelLink;
                
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
}