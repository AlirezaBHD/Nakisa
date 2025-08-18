using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IBotFlowDispatcher
{
    Task DispatchAsync(ITelegramBotClient bot, Update update, CancellationToken ct);
}