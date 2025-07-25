using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Extensions;

public static class UpdateExtensions
{
    public static long GetChatId(this Update update)
    {
        return update.Message?.Chat.Id
               ?? update.CallbackQuery?.Message?.Chat.Id
               ?? update.InlineQuery?.From?.Id
               ?? update.Message?.From?.Id
               ?? 0;
    }

    public static string? GetUsername(this Update update)
    {
        return update.Message?.From?.Username
               ?? update.CallbackQuery?.From?.Username;
    }
}
