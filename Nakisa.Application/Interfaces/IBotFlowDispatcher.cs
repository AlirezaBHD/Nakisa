using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Interfaces;

public interface IBotFlowDispatcher
{
    Task DispatchAsync(ITelegramBotClient bot, Message message, CancellationToken ct);
}