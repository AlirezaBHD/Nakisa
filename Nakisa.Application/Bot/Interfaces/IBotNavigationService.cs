using Telegram.Bot;

namespace Nakisa.Application.Bot.Interfaces;

public interface IBotNavigationService
{
    Task SendHomePageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct);
}