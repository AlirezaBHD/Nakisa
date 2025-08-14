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
    private readonly IBotNavigationService _navigation;
    public ChannelPrefixStepHandler(IUserService userService, IBotNavigationService navigation)
    {
        _userService = userService;
        _navigation = navigation;
    }
    public RegisterStep Step => RegisterStep.ChannelPrefix;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callbackData = update.CallbackQuery!.Data;
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        
        if (callbackData == "Yes")
        {
            await HandleYesOptionAsync(data, chatId, messageId, bot, ct);
        }
        else if (callbackData == "No")
        {
            await CompleteRegistrationAsync(data, chatId, messageId, bot, ct);
        }
    }
    private async Task HandleYesOptionAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot, CancellationToken ct)
    {
        data.Step = RegisterStep.ChannelLinkPrefix;
        data.ChannelIncluding = ChannelIncludingType.ChannelNameWithLink;

        if (string.IsNullOrEmpty(data.PersonChannelLink))
        {
            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "لینک چنلتو بفرست",
                cancellationToken: ct);
        }
        else
        {
            await CompleteRegistrationAsync(data, chatId, messageId, bot, ct);
        }
    }

    private async Task CompleteRegistrationAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot, CancellationToken ct)
    {
        await _userService.AddOrUpdate(data);

        data.Step = RegisterStep.Completed;

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "ثبت نام موفق",
            cancellationToken: ct);

        await _navigation.SendHomePageAsync(bot, chatId, ct);
    }
}