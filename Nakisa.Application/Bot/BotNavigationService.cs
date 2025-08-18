using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Interfaces;
using Telegram.Bot;

namespace Nakisa.Application.Bot;

public class BotNavigationService : IBotNavigationService
{
    private readonly IUserSessionService _session;
    private readonly IUserService _userService;
    private readonly string _animationId;

    public BotNavigationService(IUserSessionService session, IUserService userService)
    {
        _session = session;
        _userService = userService;
        _animationId = "CgACAgQAAxkBAAIJA2ii5LtNL3K8LmZlVXSjtmrSEnFrAAJ8HAACrdMZUY8MXCkceLdVNgQ";
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
        
        await bot.SendAnimation(
            chatId: chatId,
            animation: _animationId,
            cancellationToken: ct);

        await bot.SendMessage(
            chatId: chatId,
            text: welcomeMessage,
            replyMarkup: keyboard,
            cancellationToken: ct);
    }
}