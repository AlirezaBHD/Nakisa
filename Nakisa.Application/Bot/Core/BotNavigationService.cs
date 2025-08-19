using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.Interfaces;
using Telegram.Bot;

namespace Nakisa.Application.Bot.Core;

public class BotNavigationService : IBotNavigationService
{
    private readonly IUserSessionService _session;
    private readonly IUserService _userService;
    private const string AnimationId = "CgACAgQAAxkBAAMFaKMItuYXHIEG1d-6IxOl6ug0F1AAAnwcAAKt0xlRMYjVTuuqo7c2BA";
    private const string AboutPhotoId =
        "AgACAgQAAxkBAAMfaKMKnHb3cidvtMx6tpIcPUL6X-YAAsnRMRshbhhR-fr32o1BI2IBAAMCAANzAAM2BA";

    public BotNavigationService(IUserSessionService session, IUserService userService)
    {
        _session = session;
        _userService = userService;
    }

    public async Task SendHomePageAsync(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var isUserExist = await _userService.IsUserExist(chatId);

        _session.Clear(chatId);

        var keyboard = isUserExist
            ? MainPageKeyboard.OldUserMainMenuButton()
            : MainPageKeyboard.NewUserMainMenuButton();

        if (!isUserExist)
            await SendAboutMessage(bot, chatId, ct);

        await bot.SendAnimation(chatId, AnimationId, cancellationToken: ct);

        var welcomeMessage = isUserExist
            ? "سلام! یکی از گزینه‌های زیر رو انتخاب کن"
            : "سلام خوش اومدی!\nیکی از گزینه‌های زیر رو انتخاب کن";

        await bot.SendMessage(chatId, welcomeMessage, replyMarkup: keyboard, cancellationToken: ct);
    }

    public async Task SendHelpPageAsync(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        await SendAboutMessage(bot, chatId, ct);
        await SendNewFeaturesMessage(bot, chatId, ct);
    }

    private static async Task SendAboutMessage(ITelegramBotClient bot, long chatId, CancellationToken ct) =>
        await bot.SendPhoto(chatId, AboutPhotoId,
            caption:
            "ثبت نام کن و موزیک به اشتراک بذار\n هم چنلتو پروموت میکنی هم کسایی که سلیقه موسیقیشون مثل خودته رو پیدا میکنی",
            cancellationToken: ct);

    private static async Task SendNewFeaturesMessage(ITelegramBotClient bot, long chatId, CancellationToken ct) =>
        await bot.SendMessage(chatId,
            text:
            "فیچر های جدید هم توی راهن\n- ساخت پلیلیست اختصاصی\n- درست کردن لیست علاقه مندی ها\n- گرفتن امتیاز بر اساس ریکشن ها",
            cancellationToken: ct);

    public async Task SendInvalidCommandMessage(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var isUserExist = await _userService.IsUserExist(chatId);

        var keyboard = isUserExist
            ? MainPageKeyboard.OldUserMainMenuButton()
            : MainPageKeyboard.NewUserMainMenuButton();

        await bot.SendMessage(chatId, "دستور نامعتبره. لطفاً از منو استفاده کن.",
            replyMarkup: keyboard, cancellationToken: ct);
    }
}