using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.DTOs;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IRegisterStepHandler
{
    RegisterStep Step { get; }
    Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct);
}
