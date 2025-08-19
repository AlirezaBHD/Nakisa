using Telegram.Bot;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IBotNavigationService
{
    Task SendHomePageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct);

    Task SendHelpPageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct);

    Task SendInvalidCommandMessage(ITelegramBotClient bot,
        long chatId,
        CancellationToken ct);
}