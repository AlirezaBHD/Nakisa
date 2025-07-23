using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;

namespace Nakisa.Application.Services;

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

        // نگه داشتن بات
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        if (update.Message is not { Text: { } messageText }) return;

        var chatId = update.Message.Chat.Id;
        var user = update.Message.From;
        using var scope = _scopeFactory.CreateScope();
        
        var dispatcher = scope.ServiceProvider.GetRequiredService<IBotFlowDispatcher>();
        
        await dispatcher.DispatchAsync(botClient, update.Message, ct);
        
        // _logger.LogInformation("Message from {0}: {1}", user?.Username, messageText);

        // await botClient.SendMessage(chatId, $"🔁 شما گفتید:\n{messageText}", cancellationToken: ct);
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
