using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Services;

public class RegisterFlowHandler : IRegisterFlowHandler
{
    private readonly IUserSessionService _sessionService;

    public RegisterFlowHandler(IUserSessionService sessionService)
    {
        _sessionService = sessionService;
    }
    public async Task StartAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct)
    {
        var chatId = message.Chat.Id;

        session.Flow = UserFlow.Registering;
        session.FlowData = new RegisterDto
        {
            Step = RegisterStep.AwaitingName
        };

        _sessionService.Update(session);

        await bot.SendMessage(chatId, "لطفاً اسم خودتو وارد کن:", cancellationToken: ct);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Message message, UserSession session, CancellationToken ct)
    {
        var chatId = message.Chat.Id;
        var data = session.FlowData as RegisterDto;
        if (data == null)
        {
            await bot.SendMessage(chatId, "خطایی رخ داده. لطفاً /start را بزن.", cancellationToken: ct);
            session.Flow = UserFlow.None;
            session.FlowData = null;
            _sessionService.Update(session);
            return;
        }

        switch (data.Step)
        {
            case RegisterStep.AwaitingName:
                data.Name = message.Text;
                data.Step = RegisterStep.AwaitingEmail;
                await bot.SendMessage(chatId, "جنسیتت رو انتخاب کن:", replyMarkup: GenderMarkup(), cancellationToken: ct);
                break;

            case RegisterStep.AwaitingEmail:
                if (message.Text != "مرد" && message.Text != "زن")
                {
                    await bot.SendMessage(chatId, "لطفاً فقط یکی از گزینه‌ها رو انتخاب کن.", replyMarkup: GenderMarkup(), cancellationToken: ct);
                    return;
                }
                data.Email = message.Text;
                data.Step = RegisterStep.AwaitingPhone;
                await bot.SendMessage(chatId, "سبک موسیقی مورد علاقت چیه؟", replyMarkup: GenreMarkup(), cancellationToken: ct);
                break;

            case RegisterStep.AwaitingPhone:
                data.Phone = message.Text;

                // ذخیره در دیتابیس
                // await _uow.Users.AddAsync(new User
                // {
                //     TelegramId = chatId,
                //     Name = data.Name!,
                //     Gender = data.Gender!,
                //     FavoriteGenre = data.Genre!
                // }, ct);
                // await _uow.CommitAsync(ct);

                await bot.SendMessage(chatId, "ثبت‌نام با موفقیت انجام شد 🎉", replyMarkup: new ReplyKeyboardRemove(), cancellationToken: ct);

                session.Flow = UserFlow.None;
                session.FlowData = null;
                _sessionService.Update(session);
                break;
        }
    }
    
    private ReplyKeyboardMarkup GenderMarkup() => new(new[]
    {
        new[] { new KeyboardButton("مرد"), new KeyboardButton("زن") }
    }) { ResizeKeyboard = true };

    private ReplyKeyboardMarkup GenreMarkup() => new(new[]
    {
        new[] { new KeyboardButton("پاپ"), new KeyboardButton("رپ") },
        new[] { new KeyboardButton("سنتی"), new KeyboardButton("جز") }
    }) { ResizeKeyboard = true };
}