using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Interfaces;

public interface IPlaylistBrowseStepHandler
{
    PlaylistBrowseStep Step { get; }
    Task HandleAsync(Update update, PlaylistBrowseDto data, ITelegramBotClient bot, CancellationToken ct);
}