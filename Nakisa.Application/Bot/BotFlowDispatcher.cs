using Nakisa.Application.Bot.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot;

public class BotFlowDispatcher : IBotFlowDispatcher
{
    private readonly IUserSessionService _sessionService;
    private readonly IRegisterFlowHandler _registerHandler;
    private readonly ISongSubmitFlowHandler _songHandler;
    private readonly IPlaylistBrowseFlowHandler _playlistHandler;

    public BotFlowDispatcher(
        IUserSessionService sessionService,
        IRegisterFlowHandler registerHandler,
        ISongSubmitFlowHandler songHandler,
        IPlaylistBrowseFlowHandler playlistHandler)
    {
        _sessionService = sessionService;
        _registerHandler = registerHandler;
        _songHandler = songHandler;
        _playlistHandler = playlistHandler;
    }

    public async Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        long chatId = update.Message?.Chat.Id
                      ?? update.CallbackQuery?.Message?.Chat.Id
                      ?? update.InlineQuery?.From?.Id
                      ?? update.Message?.From?.Id
                      ?? 0;
        var session = _sessionService.GetOrCreate(chatId);

        if (session.Flow == UserFlow.None && update.Message != null)
        {
            switch (update.Message.Text?.ToLower())
            {
                case "/start":
                    await bot.SendMessage(chatId, "سلام خوش اومدی! یکی از گزینه‌های زیر رو انتخاب کن:",
                        replyMarkup: MainMenuMarkup(), cancellationToken: ct);
                    return;

                case "ثبت نام":
                    session.Flow = UserFlow.Registering;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _registerHandler.StartAsync(bot, update, session, ct);
                    return;

                case "ارسال آهنگ":
                    session.Flow = UserFlow.SubmittingSong;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _songHandler.StartAsync(bot, update, session, ct);
                    return;

                case "دیدن پلی‌لیست‌ها":
                    session.Flow = UserFlow.BrowsingPlaylists;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _playlistHandler.StartAsync(bot, update, session, ct);
                return;

                default:
                    await bot.SendMessage(chatId, "دستور نامعتبره. لطفاً از منو استفاده کن.",
                        replyMarkup: MainMenuMarkup(), cancellationToken: ct);
                    return;
            }
        }
        else
        {
            switch (session.Flow)
            {
                case UserFlow.Registering:
                    await _registerHandler.HandleAsync(bot, update, session, ct);
                    break;

                case UserFlow.SubmittingSong:
                    await _songHandler.HandleAsync(bot, update, session, ct);
                    break;

                case UserFlow.BrowsingPlaylists:
                    await _playlistHandler.HandleAsync(bot, update, session, ct);
                    break;
            }
        }
    }

    private ReplyKeyboardMarkup MainMenuMarkup() => new ReplyKeyboardMarkup(new[]
    {
        new[] { new KeyboardButton("ثبت نام") },
        new[] { new KeyboardButton("ارسال آهنگ") },
        new[] { new KeyboardButton("دیدن پلی‌لیست‌ها") }
    }) { ResizeKeyboard = true };
}