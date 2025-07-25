using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Session;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.PlaylistBrowse;

public class PlaylistBrowseFlowHandler : IPlaylistBrowseFlowHandler
{
    public Task StartAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task HandleAsync(ITelegramBotClient bot, Update update, UserSession session, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}