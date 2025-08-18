using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Type = Nakisa.Application.Bot.Register.Constants.CallbackTypes;

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
        var callbackData = update.GetCallbackData();
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();

        switch (callbackData)
        {
            case Type.Yes:
                await HandleYesOptionAsync(data, chatId, messageId, bot, ct);
                break;
            case Type.No:
                await CompleteRegistrationAsync(data, chatId, messageId, bot, ct);
                break;
        }
    }

    private async Task HandleYesOptionAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot,
        CancellationToken ct)
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

    private async Task CompleteRegistrationAsync(RegisterDto data, long chatId, int messageId, ITelegramBotClient bot,
        CancellationToken ct)
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