using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Application.DTOs.Playlist;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        if (update.Message?.Audio is not { } audio)
        {
            await SendText(bot, chatId, "یه فایل موزیک قابل قبول بفرستید", ct);
            return;
        }

        var fileId = audio.FileId;
        var caption = await _userService.GenerateCaption(chatId);
        var playlistsInfo = await _playlistService.GetPlaylistsInfo(data.TargetPlaylistId);

        var childMessage = await SendAudioToChannel(bot, playlistsInfo.TelegramChannelId, fileId, caption, ct);
        var resultMessage = FormatMessage(playlistsInfo, childMessage);

        if (playlistsInfo.Parent is not null)
        {
            var parentMessage = await SendAudioToChannel(bot, playlistsInfo.Parent.TelegramChannelId, fileId, caption, ct);
            resultMessage = FormatParentMessage(playlistsInfo, parentMessage, resultMessage);
        }

        await SendText(bot, chatId, resultMessage, ct);
        await SendText(bot, chatId, "بازم میخوای آهنگی بفرستی به این پلیلیست؟", ct);
    }
    private async Task<Message> SendAudioToChannel(ITelegramBotClient bot, long channelId, string fileId, string caption, CancellationToken ct)
    {
        return await bot.SendAudio(
            chatId: channelId,
            audio: fileId,
            caption: caption,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }

    private string FormatMessage(PlaylistsDto info, Message message)
    {
        var emoji = string.IsNullOrWhiteSpace(info.Emoji) ? "" : info.Emoji + " ";
        
        return $"{emoji}{info.Name}: <a href=\"{message.MessageLink()}\">لینک موزیک</a>";
    }

    private string FormatParentMessage(PlaylistsDto info, Message parentMessage, string childMessage)
    {
        var parent = info.Parent!;
        return $"{parent.Emoji} {parent.Name}: <a href=\"{parentMessage.MessageLink()}\">لینک موزیک</a>\n{childMessage}";
    }

    private async Task SendText(ITelegramBotClient bot, long chatId, string text, CancellationToken ct)
    {
        await bot.SendMessage(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }

}