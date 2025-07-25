using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Interfaces;

public interface IBotFlowDispatcher
{
    Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct);
}