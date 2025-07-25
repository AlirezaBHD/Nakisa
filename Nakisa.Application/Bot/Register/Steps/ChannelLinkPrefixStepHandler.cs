using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;


public class ChannelLinkPrefixStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.ChannelLinkPrefix;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var channelLink = update.Message!.Text;
        //validate Channel Link
                
        data.PersonChannelLink = channelLink;

        data.Step = RegisterStep.Completed;
        await bot.EditMessageText(
            chatId: chatId,
            messageId: update.Message!.MessageId,
            text: "ثبت نام موفق",
            cancellationToken: ct);
    }
}