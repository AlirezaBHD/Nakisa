using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Session;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.SongSubmit;

public class SongSubmitFlowHandler : ISongSubmitFlowHandler
{
    public Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}