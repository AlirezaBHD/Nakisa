using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register;

public class RegisterFlowHandler : IRegisterFlowHandler
{
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

        await bot.SendMessage(
            chatId: update.GetChatId(),
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
