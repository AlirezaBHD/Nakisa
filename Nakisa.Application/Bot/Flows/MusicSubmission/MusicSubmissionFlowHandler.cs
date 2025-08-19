using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Core.Session;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Flows.MusicSubmission;

public class MusicSubmissionFlowHandler : IFlowHandler
{
    public UserFlow Flow => UserFlow.MusicSubmission;

    private readonly Dictionary<MusicSubmissionStep, IMusicSubmissionStepHandler> _handlers;
    private readonly IUserSessionService _sessionService;
    private readonly ICategoryService _categoryService;

    public MusicSubmissionFlowHandler(IUserSessionService sessionService, IEnumerable<IMusicSubmissionStepHandler> handlers, ICategoryService categoryService)
    {
        _sessionService = sessionService;
        _categoryService = categoryService;
        _handlers = handlers.ToDictionary(h => h.Step);
    }

    public async Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        session.Flow = UserFlow.MusicSubmission;
        session.FlowData = new SongSubmissionDto() { Step = MusicSubmissionStep.SelectingPlaylist };
        _sessionService.Update(session);
        
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();

        var categories = await _categoryService.GetCategories();

        var buttons = MusicSubmissionKeyboard.CategoriesButton(categories);
        
        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "یه دسته بندی یا پلیلیست انتخاب کنید",
            replyMarkup: buttons,
            cancellationToken: ct);
    }

    public async Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        var data = session.FlowData as SongSubmissionDto;
        if (data == null)
        {
            await bot.SendMessage(update.GetChatId(), "خطا. لطفاً /start را بزنید.", cancellationToken: ct);
            session.Flow = UserFlow.None;
            session.FlowData = null;
            _sessionService.Update(session);
            return;
        }

        if (_handlers.TryGetValue(data.Step, out var handler))
        {
            await handler.HandleAsync(update, data, bot, ct);
        }

        _sessionService.Update(session);
    }
}
