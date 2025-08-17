using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.DTOs.Playlist;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Nakisa.Application.Bot.MusicSubmission.Steps;

public class WaitingForMusicStepHandler : IMusicSubmissionStepHandler
{
    private readonly IPlaylistService _playlistService;
    private readonly IMusicService _musicService;
    private readonly IUserService _userService;
    private readonly IBotNavigationService _botNavigation;

    public WaitingForMusicStepHandler(IPlaylistService playlistService, IUserService userService,
        IMusicService musicService, IBotNavigationService botNavigation)
    {
        _playlistService = playlistService;
        _userService = userService;
        _musicService = musicService;
        _botNavigation = botNavigation;
    }

    public Domain.Enums.MusicSubmissionStep Step => Domain.Enums.MusicSubmissionStep.WaitingForMusic;

    public async Task HandleAsync(Update update, SongSubmissionDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        if (update.Message?.Audio is not { } audio)
        {
            await SendTextAsync(bot, chatId, "یه فایل موزیک قابل قبول بفرستید", ct);
            return;
        }

        await HandleAudioAsync(bot, chatId, audio, data, ct);
    }
    
    private async Task HandleAudioAsync(ITelegramBotClient bot, long chatId, Audio audio, SongSubmissionDto data, CancellationToken ct)
    {
        var userIdentifier = await _userService.GenerateCaption(chatId);
        var fileId = audio.FileId;

        var playlistsInfo = await _playlistService.GetPlaylistsInfo(data.TargetPlaylistId);

        await _musicService.AddMusicAsync(audio, chatId, data.TargetPlaylistId);

        await ForwardMusicToPlaylists(bot, fileId, userIdentifier, playlistsInfo, chatId, ct);

        var backToHomeButton = MusicSubmissionKeyboard.BackToHomeButton();

        await SendTextAsync(bot, chatId, "بازم میخوای آهنگی بفرستی به این پلیلیست؟\nآهنگتو بفرست", ct, backToHomeButton);
    }
    private async Task ForwardMusicToPlaylists(
        ITelegramBotClient bot,
        string fileId,
        string userIdentifier,
        IEnumerable<PlaylistsDto> playlists,
        long chatId,
        CancellationToken ct)
    {
        foreach (var playlist in playlists)
        {
            var caption = BuildCaption(userIdentifier, playlist.Emoji, playlist.Name, playlist.ChannelInviteLink);

            var joinButton = MusicSubmissionKeyboard.JoinButton(playlist.ChannelInviteLink);

            var channelMessage = await SendAudioToChannelAsync(bot, playlist.TelegramChannelId, fileId, caption, ct);

            var resultMessage = FormatResultMessage(playlist, channelMessage);
            await SendTextAsync(bot, chatId, resultMessage, ct, joinButton);
        }
    }
    
    private static string BuildCaption(string userIdentifier, string? emoji, string name, string inviteLink)
    {
        var emojiPart = emoji.EmojiHandler();
        var linkPart = $"- {emojiPart}{name}".ToTelegramLink(inviteLink);
        return $"{userIdentifier}\n\n{linkPart}";
    }

    private static async Task<Message> SendAudioToChannelAsync(
        ITelegramBotClient bot,
        long channelId,
        string fileId,
        string caption,
        CancellationToken ct)
    {
        return await bot.SendAudio(
            chatId: channelId,
            audio: fileId,
            caption: caption,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }
    
    
    private static string FormatResultMessage(PlaylistsDto info, Message message)
    {
        var emojiPart = info.Emoji.EmojiHandler();
        var linkPart = "لینک موزیک".ToTelegramLink(message.MessageLink()!);
        return $"{emojiPart}{info.Name}: {linkPart}";
    }

    private static async Task SendTextAsync(
        ITelegramBotClient bot,
        long chatId,
        string text,
        CancellationToken ct,
        InlineKeyboardMarkup? button = null)
    {
        await bot.SendMessage(
            chatId: chatId,
            text: text,
            replyMarkup: button,
            parseMode: ParseMode.Html,
            cancellationToken: ct);
    }
}