using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
    
    public static long? GetUserId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message?.From?.Id,
            UpdateType.CallbackQuery => update.CallbackQuery?.From?.Id,
            UpdateType.InlineQuery => update.InlineQuery?.From?.Id,
            UpdateType.ChosenInlineResult => update.ChosenInlineResult?.From?.Id,
            UpdateType.MyChatMember => update.MyChatMember?.From?.Id,
            UpdateType.ChatMember => update.ChatMember?.From?.Id,
            UpdateType.ChatJoinRequest => update.ChatJoinRequest?.From?.Id,
            _ => null
        };
    }
}
