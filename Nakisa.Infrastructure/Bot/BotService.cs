using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nakisa.Application.Bot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Nakisa.Infrastructure.Bot;

public class BotService : BackgroundService
{
    private readonly ILogger<BotService> _logger;
    private readonly TelegramBotClient _bot;
    private readonly IServiceScopeFactory _scopeFactory;


    public BotService(ILogger<BotService> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        var token = config["TelegramBot:Token"];
        _bot = new TelegramBotClient(token!);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new Telegram.Bot.Polling.ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            },
            cancellationToken: stoppingToken
        );

        var me = await _bot.GetMe(cancellationToken: stoppingToken);
        _logger.LogInformation("Bot started: {0}", me.Username);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var dispatcher = scope.ServiceProvider.GetRequiredService<IBotFlowDispatcher>();
        
        await dispatcher.DispatchAsync(botClient, update, ct);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception ex, CancellationToken ct)
    {
        var err = ex switch
        {
            ApiRequestException apiEx => $"Telegram API Error:\n[{apiEx.ErrorCode}]\n{apiEx.Message}",
            _ => ex.ToString()
        };
        _logger.LogError(err);
        return Task.CompletedTask;
    }
}
