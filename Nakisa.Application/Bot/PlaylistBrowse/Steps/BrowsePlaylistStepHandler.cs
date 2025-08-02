using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.PlaylistBrowse.Steps;

public class BrowsePlaylistStepHandler : IPlaylistBrowseStepHandler
{
    private readonly ICategoryService _categoryService;
    private readonly IPlaylistService _playlistService;

    public BrowsePlaylistStepHandler(ICategoryService categoryService, IPlaylistService playlistService)
    {
        _categoryService = categoryService;
        _playlistService = playlistService;
    }

    public PlaylistBrowseStep Step => PlaylistBrowseStep.BrowsePlaylist;

    public async Task HandleAsync(Update update, PlaylistBrowseDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var messageId = update.GetMessageId();
        var callbackData = update.CallbackQuery?.Data ?? string.Empty;

        if (callbackData.StartsWith("category"))
        {
            var categoryId = int.Parse(callbackData.Split(":")[1]);
            var playlists = await _playlistService.GetPlaylistsByCategoryId(categoryId);
            var buttons = PlaylistBrowseKeyboard.CategoryPlaylistsButton(playlists);

            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "یه پلیلیست انتخاب کنید",
                replyMarkup: buttons,
                cancellationToken: ct);
        }

        else if (callbackData.StartsWith("playlist"))
        {
            var playlistId = int.Parse(callbackData.Split(":")[1]);
            var playlists = await _playlistService.GetPlaylistsInfoByParentId(playlistId);

            var buttons = PlaylistBrowseKeyboard.PlaylistsButton(playlists);

            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "یه پلیلیست انتخاب کنید",
                replyMarkup: buttons,
                cancellationToken: ct);
        }

        else
        {
            var categories = await _categoryService.GetCategories();

            var buttons = PlaylistBrowseKeyboard.CategoriesButton(categories);

            await bot.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: "یه دسته بندی یا پلیلیست انتخاب کنید",
                replyMarkup: buttons,
                cancellationToken: ct);
        }
    }
}