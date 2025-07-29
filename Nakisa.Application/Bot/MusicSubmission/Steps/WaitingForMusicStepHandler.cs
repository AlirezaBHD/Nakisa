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
        var chatId = update.GetChatId();
        
        if (update.Message?.Audio != null)
        {
            var audio = update.Message.Audio;
            var fileId = audio.FileId;
            // var caption = _userService.GenerateCaption(chatId);
            var playlistsInfo = await _playlistService.GetPlaylistsInfo(data.TargetPlaylistId);
            
            // var result = _botService.SendMusicToChannel(fileId: fileId, caption: caption, playlists: []);
            await bot.SendMessage(
                chatId: chatId,
                text: "یه فایل موزیک قابل قبول بفرستید",
                cancellationToken: ct);
            await bot.SendMessage(
                chatId: chatId,
                text: "یه فایل موزیک قابل قبول بفرستید",
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "یه فایل موزیک قابل قبول بفرستید",
                cancellationToken: ct);
        }

    }
}