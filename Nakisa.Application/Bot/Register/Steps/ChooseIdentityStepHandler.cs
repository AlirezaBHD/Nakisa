using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChooseIdentityStepHandler : IRegisterStepHandler
{
    private readonly IUserService _userService;
    private readonly IBotNavigationService _navigation;
    public ChooseIdentityStepHandler(IUserService userService, IBotNavigationService navigation)
    {
        _userService = userService;
        _navigation = navigation;
    }
    public RegisterStep Step => RegisterStep.ChooseIdentity;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var callback = update.CallbackQuery!.Data;
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        
        data.TelegramId  = update.GetUserId();
        data.ChatId = chatId;
        
        switch (callback)
        {
            case "TelegramName":

                data.FirstName = update.CallbackQuery?.From.FirstName;
                data.LastName = update.CallbackQuery?.From.LastName;
                
                data.CaptionIdentifier = CaptionIdentifierType.TelegramName;
                data.Step = RegisterStep.ChooseLinkType;
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "می‌خوای وقتی کسی روی اسمت کلیک کرد، به جایی وصل بشه؟",
                    replyMarkup: RegisterKeyboards.LinkTypeButton(),
                    cancellationToken: ct);
                break;

            case "Nickname":
                data.CaptionIdentifier = CaptionIdentifierType.Nickname;
                data.Step = RegisterStep.ChoosingNickname;
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "اسم مستعارت رو وارد کن",
                    cancellationToken: ct);
                break;

            case "Unknown":
                data.CaptionIdentifier = CaptionIdentifierType.Unknown;
                await _userService.AddOrUpdate(data);
                data.Step = RegisterStep.Completed;
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "ثبت نام موفق",
                    cancellationToken: ct);
                
                await _navigation.SendHomePageAsync(bot, chatId, ct);

                break;
        }
    }
}
