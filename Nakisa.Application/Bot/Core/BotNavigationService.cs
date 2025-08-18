using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Interfaces;
using Telegram.Bot;

namespace Nakisa.Application.Bot.Core;

public class BotNavigationService : IBotNavigationService
{
    private readonly IUserSessionService _session;
    private readonly IUserService _userService;
    private readonly string _animationId;

    public BotNavigationService(IUserSessionService session, IUserService userService)
    {
        _session = session;
        _userService = userService;
        _animationId = "CgACAgQAAxkBAAMFaKMItuYXHIEG1d-6IxOl6ug0F1AAAnwcAAKt0xlRMYjVTuuqo7c2BA";
    }

    public async Task SendHomePageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct)
    {
        var isUserExist = await _userService.IsUserExist(chatId);
        var welcomeMessage = isUserExist
            ? "سلام! یکی از گزینه‌های زیر رو انتخاب کن"
            : "سلام خوش اومدی!\nیکی از گزینه‌های زیر رو انتخاب کن";

        _session.Clear(chatId);
        _session.Clear(chatId);

        var keyboard = isUserExist
            ? MainPageKeyboard.OldUserMainMenuButton()
            : MainPageKeyboard.NewUserMainMenuButton();

        if (!isUserExist)
        {
            await SendAboutMessage(bot, chatId, ct);
        }
        
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
    
    public async Task SendHelpPageAsync(
        ITelegramBotClient bot,
        long chatId,
        CancellationToken ct)
    {

        await SendAboutMessage(bot, chatId, ct);
        await SendNewFeaturesMessage(bot, chatId, ct);
    }

    private async Task SendAboutMessage(ITelegramBotClient bot,
        long chatId,
        CancellationToken ct)
    {
        await bot.SendPhoto(
            chatId: chatId,
            photo: "AgACAgQAAxkBAAMfaKMKnHb3cidvtMx6tpIcPUL6X-YAAsnRMRshbhhR-fr32o1BI2IBAAMCAANzAAM2BA",
            caption: "ثبت نام کن و موزیک به اشتراک بذار\n هم چنلتو پروموت میکنی هم کسایی که سلیقه موسیقیشون مثل خودته رو پیدا میکنی",
            cancellationToken: ct);
    }
    private async Task SendNewFeaturesMessage(ITelegramBotClient bot,
        long chatId,
        CancellationToken ct)
    {
        await bot.SendMessage(
            chatId:chatId,
            text: "فیچر های جدید هم توی راهن\n- ساخت پلیلیست اختصاصی\n- درست کردن لیست علاقه مندی ها\n- گرفتن امتیاز بر اساس ریکشن ها",
            cancellationToken: ct);
    }
    
}