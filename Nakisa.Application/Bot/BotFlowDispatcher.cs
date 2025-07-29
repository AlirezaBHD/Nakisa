using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot;

public class BotFlowDispatcher : IBotFlowDispatcher
{
    private readonly IUserSessionService _sessionService;
    private readonly IRegisterFlowHandler _registerHandler;
    private readonly IMusicSubmitFlowHandler _musicHandler;
    private readonly IPlaylistBrowseFlowHandler _playlistHandler;
    private readonly IUserService _userService;

    public BotFlowDispatcher(
        IUserSessionService sessionService,
        IRegisterFlowHandler registerHandler,
        IMusicSubmitFlowHandler musicHandler,
        IPlaylistBrowseFlowHandler playlistHandler,
        IUserService userService)
    {
        _sessionService = sessionService;
        _registerHandler = registerHandler;
        _musicHandler = musicHandler;
        _playlistHandler = playlistHandler;
        _userService = userService;
    }

    public async Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var session = _sessionService.GetOrCreate(chatId);

        if (session.Flow == UserFlow.None)
        {
            if (IsStartCommand(update))
            {
                await HandleStartCommandAsync(bot, chatId, ct);
                return;
            }

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

    private bool IsStartCommand(Update update)
    {
        return update.Message?.Text?.Trim().ToLower() == "/start";
    }

    private async Task HandleStartCommandAsync(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var isUserExist = await _userService.IsUserExist(chatId);
        var welcomeMessage = isUserExist
            ? "سلام! یکی از گزینه‌های زیر رو انتخاب کن:"
            : "سلام خوش اومدی! یکی از گزینه‌های زیر رو انتخاب کن:";

        var keyboard = isUserExist
            ? MainKeyboard.OldUserMainMenuButton()
            : MainKeyboard.NewUserMainMenuButton();

        await bot.SendMessage(chatId, welcomeMessage, replyMarkup: keyboard, cancellationToken: ct);
    }

    private async Task HandleCallbackCommandAsync(ITelegramBotClient bot, Update update, UserSession session,
        string callbackData, CancellationToken ct)
    {
        switch (callbackData)
        {
            case "Register":
                await StartFlowAsync(bot, update, session, UserFlow.Registering, _registerHandler.StartAsync, ct);
                break;

            case "SubmitSong":
                await StartFlowAsync(bot, update, session, UserFlow.SubmittingSong, _musicHandler.StartAsync, ct);
                break;

            case "ViewPlaylists":
                await StartFlowAsync(bot, update, session, UserFlow.BrowsingPlaylists, _playlistHandler.StartAsync, ct);
                break;

            default:
                await SendInvalidCommandMessage(bot, update.GetChatId(), ct);
                break;
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