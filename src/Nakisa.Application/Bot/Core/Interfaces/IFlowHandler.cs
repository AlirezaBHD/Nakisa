using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Session;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IFlowHandler
{
    UserFlow Flow { get; }
    Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct);
    Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct);
}