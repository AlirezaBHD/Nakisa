using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Core.Session;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Flows.Register;

public class RegisterFlowHandler : IFlowHandler
{
    public UserFlow Flow => UserFlow.Registering;
    
    private readonly Dictionary<RegisterStep, IRegisterStepHandler> _handlers;
    private readonly IUserSessionService _sessionService;

    public RegisterFlowHandler(IUserSessionService sessionService, IEnumerable<IRegisterStepHandler> handlers)
    {
        _sessionService = sessionService;
        _handlers = handlers.ToDictionary(h => h.Step);
    }

    public async Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        session.Flow = UserFlow.Registering;
        session.FlowData = new RegisterDto { Step = RegisterStep.ChooseIdentity };
        _sessionService.Update(session);
        
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        
        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "دوست داری اسمت چجوری نمایش داده بشه؟",
            replyMarkup: RegisterKeyboards.ChooseIdentityButton(),
            cancellationToken: ct);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        var data = session.FlowData as RegisterDto;
        if (data == null)
        {
            await bot.SendMessage(update.GetChatId(), "خطا. لطفاً /start را بزنید.", cancellationToken: ct);
            session.Flow = UserFlow.None;
            session.FlowData = null;
            _sessionService.Update(session);
            return;
        }

        if (_handlers.TryGetValue(data.Step, out var handler))
        {
            await handler.HandleAsync(update, data, bot, ct);
        }

        _sessionService.Update(session);
    }
}
