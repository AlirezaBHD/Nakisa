using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;

public class ChoosingNicknameStepHandler : IRegisterStepHandler
{
    public RegisterStep Step => RegisterStep.ChoosingNickname;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var nickname = update.Message!.Text;

        //TODO: Check if it already exists in database
        // Validate Name

        data.Nickname = nickname;

        data.Step = RegisterStep.ChooseLinkType;

        var chatId = update.GetChatId();
        await bot.SendMessage(
            chatId: chatId,
            text: "می‌خوای وقتی کسی روی اسمت کلیک کرد، به جایی وصل بشه؟",
            replyMarkup: RegisterKeyboards.LinkTypeButton(),
            cancellationToken: ct);
    }
}