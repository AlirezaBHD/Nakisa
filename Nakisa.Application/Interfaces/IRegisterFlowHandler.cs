using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Interfaces;

public interface IRegisterFlowHandler
{
    Task StartAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct);
    Task HandleAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct);
}