using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Services;

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

    public async Task DispatchAsync(ITelegramBotClient bot, Message message, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var session = _sessionService.GetOrCreate(chatId);

        // اگر هیچ Flow فعالی نداره، چک کن کاربر چی گفته
        if (session.Flow == UserFlow.None)
        {
            switch (message.Text?.ToLower())
            {
                case "/start":
                    await bot.SendMessage(chatId, "سلام! یکی از گزینه‌های زیر رو انتخاب کن:", replyMarkup: MainMenuMarkup(), cancellationToken: ct);
                    return;

                case "ثبت نام":
                    session.Flow = UserFlow.Registering;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _registerHandler.StartAsync(bot, message, session, ct);
                    return;

                case "ارسال آهنگ":
                    session.Flow = UserFlow.SubmittingSong;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _songHandler.StartAsync(bot, message, session, ct);
                    return;

                case "دیدن پلی‌لیست‌ها":
                    session.Flow = UserFlow.BrowsingPlaylists;
                    session.FlowData = null;
                    _sessionService.Update(session);
                    await _playlistHandler.StartAsync(bot, message, session, ct);
                    return;

                default:
                    await bot.SendMessage(chatId, "دستور نامعتبره. لطفاً از منو استفاده کن.", replyMarkup: MainMenuMarkup(), cancellationToken: ct);
                    return;
            }
        }
        else
        {
            switch (session.Flow)
            {
                case UserFlow.Registering:
                    await _registerHandler.HandleAsync(bot, message, session, ct);
                    break;

                case UserFlow.SubmittingSong:
                    await _songHandler.HandleAsync(bot, message, session, ct);
                    break;

                case UserFlow.BrowsingPlaylists:
                    await _playlistHandler.HandleAsync(bot, message, session, ct);
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
