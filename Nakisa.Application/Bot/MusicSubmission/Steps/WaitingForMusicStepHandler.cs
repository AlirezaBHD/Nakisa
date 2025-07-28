using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.MusicSubmission.Steps;

public class WaitingForMusicStepHandler : IMusicSubmissionStepHandler
{
    private readonly IPlaylistService _playlistService;
    private readonly IUserService _userService;
    public WaitingForMusicStepHandler(IPlaylistService playlistService, IUserService userService)
    {
        _playlistService = playlistService;
        _userService = userService;
    }

    public Domain.Enums.MusicSubmissionStep Step => Domain.Enums.MusicSubmissionStep.WaitingForMusic;

    public async Task HandleAsync(Update update, SongSubmissionDto data, ITelegramBotClient bot, CancellationToken ct)
    {

    }
}