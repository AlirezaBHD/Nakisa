using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
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

        if (update.CallbackQuery?.Data == "home page")
        {
            await _botNavigation.SendHomePageAsync(bot, chatId, ct);
            return;
        }
        if (update.Message?.Audio is not { } audio)
        {
            await SendText(bot, chatId, "یه فایل موزیک قابل قبول بفرستید", ct);
            return;
        }

        var userIdentifier = await _userService.GenerateCaption(chatId);

        var fileId = audio.FileId;

        var playlistsInfo = await _playlistService.GetPlaylistsInfo(data.TargetPlaylistId);

        await _musicService.AddMusicAsync(audio, chatId, data.TargetPlaylistId);

        await ProcessPlaylists(playlistsInfo);

        var declineButton = new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData("لغو", "home page")
            ]
        ]);

        await SendText(bot, chatId, "بازم میخوای آهنگی بفرستی به این پلیلیست؟\n آهنگتو بفرست", ct, declineButton);

        async Task ProcessPlaylists(IEnumerable<PlaylistsDto> playlists)
        {
            foreach (var playlist in playlists)
            {
                var messageCaption = GenerateCaption(userIdentifier, playlist.Emoji, playlist.Name,
                    playlist.ChannelInviteLink);

                var button = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithUrl("عضویت", playlist.ChannelInviteLink)
                    ]
                ]);

                var childMessage =
                    await SendAudioToChannel(bot, playlist.TelegramChannelId, fileId, messageCaption, button, ct);

                var resultMessage = FormatMessage(playlist, childMessage);
                await SendText(bot, chatId, resultMessage, ct, button);
            }
        }
    }

    private static string GenerateCaption(string userIdentifier, string? emoji, string name, string inviteLink)
    {
        emoji = emoji == null ? "" : emoji + " ";

        var caption = $"{userIdentifier}\n\n<a href=\"{inviteLink}\">- {emoji}{name}</a>";

        return caption;
    }

    private async Task<Message> SendAudioToChannel(ITelegramBotClient bot, long channelId, string fileId,
        string caption, InlineKeyboardMarkup button, CancellationToken ct)
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

        return
            $"{emoji}{info.Name}: <a href=\"{message.MessageLink()}\">لینک موزیک</a>";
    }

    private async Task SendText(ITelegramBotClient bot, long chatId, string text, CancellationToken ct,
        InlineKeyboardMarkup? button = null)
    {
        if (button is not null)
        {
            await bot.SendMessage(
                chatId: chatId,
                text: text,
                replyMarkup: button,
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
        else
        {
            await bot.SendMessage(
                chatId: chatId,
                text: text,
                parseMode: ParseMode.Html,
                cancellationToken: ct);
        }
    }
}