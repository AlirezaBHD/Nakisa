using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Services;

public class SongSubmitFlowHandler : ISongSubmitFlowHandler
{
    public Task StartAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}