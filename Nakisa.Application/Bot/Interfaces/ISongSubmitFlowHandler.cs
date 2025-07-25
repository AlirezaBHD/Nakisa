using Nakisa.Application.Bot.Session;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Interfaces;

public interface ISongSubmitFlowHandler
{
    Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct);
    Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct);
}