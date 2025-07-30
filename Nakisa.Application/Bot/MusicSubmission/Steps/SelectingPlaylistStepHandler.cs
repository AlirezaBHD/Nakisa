using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.MusicSubmission.Steps;

public class SelectingPlaylistStepHandler : IMusicSubmissionStepHandler
{
    private readonly ICategoryService _categoryService;
    private readonly IPlaylistService _playlistService;

    public SelectingPlaylistStepHandler(ICategoryService categoryService, IPlaylistService playlistService)
    {
        _categoryService = categoryService;
        _playlistService = playlistService;
    }

    public MusicSubmissionStep Step => MusicSubmissionStep.SelectingPlaylist;

    public async Task HandleAsync(Update update, SongSubmissionDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        var callbackData = update.CallbackQuery?.Data ?? string.Empty;

        if (callbackData.StartsWith("category"))
        {
            var categoryId = int.Parse(callbackData.Split(":")[1]);
            var playlists = await _playlistService.GetPlaylistsByCategoryId(categoryId);
            var buttons = MusicSubmissionKeyboard.CategoryPlaylistsButton(playlists);

            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "یه پلیلیست انتخاب کنید",
                replyMarkup: buttons,
                cancellationToken: ct);
        }
        
        else if (callbackData.StartsWith("playlist"))
        {
            var playlistType = callbackData.Split(":")[2];
            if (playlistType == "brows")
            {
                var playlistId = int.Parse(callbackData.Split(":")[1]);
                var playlists = await _playlistService.GetPlaylistsByParentId(playlistId);
                
                var buttons = MusicSubmissionKeyboard.PlaylistsButton(playlists);

                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "یه پلیلیست انتخاب کنید",
                    replyMarkup: buttons,
                    cancellationToken: ct);
                
                
            }
            else if (playlistType == "submit")
            {
                var playlistId = int.Parse(callbackData.Split(":")[1]);
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: messageId,
                    text: "موزیکتو بفرست تا به پلیلیست اضافش کنم",
                    cancellationToken: ct);
                data.Step = MusicSubmissionStep.WaitingForMusic;
                data.TargetPlaylistId = playlistId;
            }
        }
        
        else
        {
            var categories = await _categoryService.GetCategories();

            var buttons = MusicSubmissionKeyboard.CategoriesButton(categories);

            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "یه دسته بندی یا پلیلیست انتخاب کنید",
                replyMarkup: buttons,
                cancellationToken: ct);
        }
    }
}