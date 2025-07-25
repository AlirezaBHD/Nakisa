using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Interfaces;

public interface IRegisterStepHandler
{
    RegisterStep Step { get; }
    Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct);
}
