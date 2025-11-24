using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Core.Session;
using Telegram.Bot;
using Telegram.Bot.Types;
using Type = Nakisa.Application.Bot.Core.Constants.MainPageCallbackTypes;

namespace Nakisa.Application.Bot.Core;

public class BotFlowDispatcher : IBotFlowDispatcher
{
    private readonly IUserSessionService _sessionService;
    private readonly Dictionary<UserFlow, IFlowHandler> _handlers;
    private readonly IBotNavigationService _navigation;

    public BotFlowDispatcher(
        IUserSessionService sessionService,
        IBotNavigationService navigation,
        IEnumerable<IFlowHandler> handlers)
    {
        _sessionService = sessionService;
        _navigation = navigation;
        _handlers = handlers.ToDictionary(h => h.Flow, h => h);
    }

    public async Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var session = _sessionService.GetOrCreate(chatId);

        if (update.IsStartOrCancelCommand())
        {
            _sessionService.Clear(chatId);
            await _navigation.SendHomePageAsync(bot, chatId, ct);
            return;
        }
        
        if (update.IsHelpCommand())
        {
            await _navigation.SendHelpPageAsync(bot, chatId, ct);
            return;
        }
        
        if (session.Flow == UserFlow.None)
        {
            var callbackData = update.CallbackQuery?.Data;

            if (string.IsNullOrEmpty(callbackData))
            {
                await _navigation.SendInvalidCommandMessage(bot, chatId, ct);
                return;
            }

            await HandleCallbackCommandAsync(bot, update, session, callbackData, ct);
            return;
        }

        await HandleFlowAsync(bot, update, session, ct);
    }

    private async Task HandleCallbackCommandAsync(ITelegramBotClient bot, Update update, UserSession session,
        string callbackData, CancellationToken ct)
    {
        var flow = callbackData switch
        {
            Type.Register     => UserFlow.Registering,
            Type.SubmitSong   => UserFlow.MusicSubmission,
            Type.ViewPlaylists => UserFlow.BrowsingPlaylists,
            _ => UserFlow.None
        };

        if (flow == UserFlow.None)
        {
            await _navigation.SendInvalidCommandMessage(bot, update.GetChatId(), ct);
        }
        else
        {
            await StartFlowAsync(bot, update, session, flow, ct);
        }
    }

    private async Task StartFlowAsync(ITelegramBotClient bot, Update update, UserSession session, UserFlow flow,
        CancellationToken ct)
    {
        session.Flow = flow;
        session.FlowData = null;
        _sessionService.Update(session);

        if (_handlers.TryGetValue(flow, out var handler))
        {
            await handler.StartAsync(bot, update, session, ct);
        }
        else
        {
            await _navigation.SendInvalidCommandMessage(bot, update.GetChatId(), ct);
        }
    }

    private async Task HandleFlowAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        if (_handlers.TryGetValue(session.Flow, out var handler))
        {
            await handler.HandleAsync(bot, update, session, ct);
        }
        else
        {
            await _navigation.SendInvalidCommandMessage(bot, update.GetChatId(), ct);
        }
    }
}