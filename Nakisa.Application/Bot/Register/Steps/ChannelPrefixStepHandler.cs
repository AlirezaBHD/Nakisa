using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChannelPrefixStepHandler : IRegisterStepHandler
{
    private readonly IUserService _userService;
    public ChannelPrefixStepHandler(IUserService userService)
    {
        _userService = userService;
    }
    public RegisterStep Step => RegisterStep.ChannelPrefix;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callBack = update.CallbackQuery!.Data;
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        
        switch (callBack)
        {
            case "Yes":
                data.Step = RegisterStep.ChannelLinkPrefix;
                data.ChannelIncluding = ChannelIncludingType.ChannelNameWithLink;
                        
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "لینک پابلیک چنلتو بفرست",
                    cancellationToken: ct);
                        
                break;
                    
            case "No":
                
                await _userService.AddOrUpdate(data);

                data.Step = RegisterStep.Completed;
                        
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "ثبت نام موفق",
                    cancellationToken: ct);
                        
                break;
                    
        }
    }
}