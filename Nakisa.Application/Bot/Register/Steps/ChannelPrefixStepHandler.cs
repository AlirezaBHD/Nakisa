using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChannelPrefixStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.ChannelPrefix;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callBack = update.CallbackQuery!.Data;
        var chatId = update.GetChatId();
        switch (callBack)
        {
            case "Yes":
                data.Step = RegisterStep.ChannelLinkPrefix;
                data.ChannelIncluding = ChannelIncludingType.ChannelNameWithLink;
                        
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: update.CallbackQuery.Message!.MessageId,
                    text: "لینک پابلیک چنلتو بفرست",
                    cancellationToken: ct);
                        
                break;
                    
            case "No":
                //create user

                data.Step = RegisterStep.Completed;
                        
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: update.CallbackQuery.Message!.MessageId,
                    text: "ثبت نام موفق",
                    cancellationToken: ct);
                        
                break;
                    
        }
    }
}