using Telegram.Bot;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IBotNavigationService
{
    Task SendHomePageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct);
}