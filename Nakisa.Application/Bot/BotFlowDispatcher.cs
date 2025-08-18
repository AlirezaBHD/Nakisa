using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Bot.Session;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Type = Nakisa.Application.Bot.Constants.CallbackTypes;

namespace Nakisa.Application.Bot;

public class BotFlowDispatcher : IBotFlowDispatcher
{
    private readonly IUserSessionService _sessionService;
    private readonly IRegisterFlowHandler _registerHandler;
    private readonly IMusicSubmitFlowHandler _musicHandler;
    private readonly IPlaylistBrowseFlowHandler _playlistHandler;
    private readonly IBotNavigationService _navigation;

    public BotFlowDispatcher(
        IUserSessionService sessionService,
        IRegisterFlowHandler registerHandler,
        IMusicSubmitFlowHandler musicHandler,
        IPlaylistBrowseFlowHandler playlistHandler, IBotNavigationService navigation)
    {
        _sessionService = sessionService;
        _registerHandler = registerHandler;
        _musicHandler = musicHandler;
        _playlistHandler = playlistHandler;
        _navigation = navigation;
    }

    public async Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var session = _sessionService.GetOrCreate(chatId);

        if (ShouldHeadToMainMenu(update))
        {
            _sessionService.Clear(chatId);
            await _navigation.SendHomePageAsync(bot, chatId, ct);
            return;
        }
        else if (session.Flow == UserFlow.None)
        {
            var callbackData = update.CallbackQuery?.Data;

            if (string.IsNullOrEmpty(callbackData))
            {
                await SendInvalidCommandMessage(bot, chatId, ct);
                return;
            }

            await HandleCallbackCommandAsync(bot, update, session, callbackData, ct);
            return;
        }

        await HandleFlowAsync(bot, update, session, ct);
    }

    private bool ShouldHeadToMainMenu(Update update)
    {
        var message = update.Message?.Text?.Trim().ToLower();
        var callbackData = update.GetCallbackData().ToLower();
        if (message == "/start" || message == "/cancel")
        {
            return true;
        }

        if (callbackData == "cancel" || callbackData == "home_page")
        {
            return true;
        }

        return false;
    }

    private async Task HandleCallbackCommandAsync(ITelegramBotClient bot, Update update, UserSession session,
        string callbackData, CancellationToken ct)
    {
        switch (callbackData)
        {
            case Type.Register:
                await FlowStarterAsync(UserFlow.Registering);
                break;

            case Type.SubmitSong:
                await FlowStarterAsync(UserFlow.SubmittingSong);
                break;

            case Type.ViewPlaylists:
                await FlowStarterAsync(UserFlow.BrowsingPlaylists);
                break;

            default:
                await SendInvalidCommandMessage(bot, update.GetChatId(), ct);
                break;
        }

        return;

        async Task FlowStarterAsync( UserFlow flow)
        {
            await StartFlowAsync(bot, update, session, flow, _musicHandler.StartAsync, ct);
        }
    }

    private async Task StartFlowAsync(ITelegramBotClient bot, Update update, UserSession session, UserFlow flow,
        Func<ITelegramBotClient, Update, UserSession, CancellationToken, Task> startHandler, CancellationToken ct)
    {
        session.Flow = flow;
        session.FlowData = null;
        _sessionService.Update(session);
        await startHandler(bot, update, session, ct);
    }

    private async Task HandleFlowAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        switch (session.Flow)
        {
            case UserFlow.Registering:
                await _registerHandler.HandleAsync(bot, update, session, ct);
                break;

            case UserFlow.SubmittingSong:
                await _musicHandler.HandleAsync(bot, update, session, ct);
                break;

            case UserFlow.BrowsingPlaylists:
                await _playlistHandler.HandleAsync(bot, update, session, ct);
                break;
        }
    }

    private async Task SendInvalidCommandMessage(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        await bot.SendMessage(chatId, "دستور نامعتبره. لطفاً از منو استفاده کن.",
            replyMarkup: MainKeyboard.NewUserMainMenuButton(), cancellationToken: ct);
    }
}