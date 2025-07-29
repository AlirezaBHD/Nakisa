using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChoosingNicknameStepHandler : IRegisterStepHandler
{
    private readonly IUserService _userService;

    public ChoosingNicknameStepHandler(IUserService userService)
    {
        _userService = userService;
    }

    public RegisterStep Step => RegisterStep.ChoosingNickname;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();

        var nickname = update.Message!.Text;

        var isNicknameValid =await IsNicknameValid(nickname, chatId);
        
        if (!isNicknameValid.IsValid)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: isNicknameValid.ErrorMessage,
                cancellationToken: ct);
        }

        else
        {
            data.Nickname = nickname;

            data.Step = RegisterStep.ChooseLinkType;

            await bot.SendMessage(
                chatId: chatId,
                text: "می‌خوای وقتی کسی روی اسمت کلیک کرد، به جایی وصل بشه؟",
                replyMarkup: RegisterKeyboards.LinkTypeButton(),
                cancellationToken: ct);
        }
    }

    private async Task<(bool IsValid, string ErrorMessage)> IsNicknameValid(string? nickname, long chatId)
    {
        var isTaken = await _userService.IsNicknameTaken(nickname!, chatId);
        if (isTaken)
        {
            return (false, "این اسم توسط یکی دیگه انتخب شده. یه اسم دیگه انتخاب کن");
        }
        else if (nickname!.Length >= 30 || nickname.Length < 2)
        {
            return (false, "نام کاربری باید بین 2 تا 30 کارکتر باشد. یه اسم دیگه انتخاب کن");
        }

        else
        {
            return (true, string.Empty);
        }
    }
}