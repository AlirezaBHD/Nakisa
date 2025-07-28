using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Interfaces;

public interface IMusicSubmissionStepHandler
{
    MusicSubmissionStep Step { get; }
    Task HandleAsync(Update update, SongSubmissionDto data, ITelegramBotClient bot, CancellationToken ct);

}