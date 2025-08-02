using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Interfaces;
using Telegram.Bot;

namespace Nakisa.Application.Bot;

public class BotNavigationService : IBotNavigationService
{
    private readonly IUserSessionService _session;
    private readonly IUserService _userService;

    public BotNavigationService(IUserSessionService session, IUserService userService)
    {
        _session = session;
        _userService = userService;
    }

    public async Task SendHomePageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct)
    {
        var isUserExist = await _userService.IsUserExist(chatId);
        var welcomeMessage = isUserExist
            ? "سلام! یکی از گزینه‌های زیر رو انتخاب کن:"
            : "سلام خوش اومدی! یکی از گزینه‌های زیر رو انتخاب کن:";

        _session.Clear(chatId);
        _session.Clear(chatId);

        var keyboard = isUserExist
            ? MainKeyboard.OldUserMainMenuButton()
            : MainKeyboard.NewUserMainMenuButton();
        
        await bot.SendMessage(chatId, welcomeMessage, replyMarkup: keyboard, cancellationToken: ct);
    }
}