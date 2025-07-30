using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Session;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.PlaylistBrowse;

public class PlaylistBrowseFlowHandler : IPlaylistBrowseFlowHandler
{
    private readonly Dictionary<PlaylistBrowseStep, IPlaylistBrowseStepHandler> _handlers;
    private readonly IUserSessionService _sessionService;
    private readonly ICategoryService _categoryService;

    public PlaylistBrowseFlowHandler(IUserSessionService sessionService, IEnumerable<IPlaylistBrowseStepHandler> handlers, ICategoryService categoryService)
    {
        _sessionService = sessionService;
        _categoryService = categoryService;
        _handlers = handlers.ToDictionary(h => h.Step);
    }

    public async Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        session.Flow = UserFlow.BrowsingPlaylists;
        session.FlowData = new PlaylistBrowseDto() { Step = PlaylistBrowseStep.BrowsePlaylist };
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
        throw new NotImplementedException();
    }

    public Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}