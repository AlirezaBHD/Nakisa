using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Extensions;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Flows.MusicSubmission.Utils;
using Nakisa.Application.Bot.Keyboards;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Types = Nakisa.Application.Bot.Core.Constants.MusicSubmissionCallbackTypes;

namespace Nakisa.Application.Bot.Flows.MusicSubmission.Steps;

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
        var callbackData = update.GetCallbackData();
        
        var parsed = CallbackDataParser.Parse(callbackData);
        switch (parsed.Type)
        {
            case Types.Category:
                await HandleCategoryAsync(parsed.Id, bot, chatId, messageId, ct);
                break;

            case Types.Playlist when parsed.Action == Types.PlaylistActions.Browse:
                await HandlePlaylistBrowseAsync(parsed.Id, bot, chatId, messageId, ct);
                break;

            case Types.Playlist when parsed.Action == Types.PlaylistActions.Submit:
                await HandlePlaylistSubmitAsync(parsed.Id, bot, chatId, messageId, data, ct);
                break;

            default:
                await ShowCategoriesAsync(bot, chatId, messageId, ct);
                break;
        }
    }

    private async Task HandleCategoryAsync(int categoryId, ITelegramBotClient bot, long chatId, int messageId,
        CancellationToken ct)
    {
        var playlists = await _playlistService.GetPlaylistsByCategoryId(categoryId);
        var buttons = MusicSubmissionKeyboard.CategoryPlaylistsButton(playlists);

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "یه پلیلیست انتخاب کنید",
            replyMarkup: buttons,
            cancellationToken: ct);
    }

    
    private async Task HandlePlaylistBrowseAsync(int playlistId, ITelegramBotClient bot, long chatId, int messageId,
        CancellationToken ct)
    {
        var playlists = await _playlistService.GetPlaylistsByParentId(playlistId);

        var buttons = MusicSubmissionKeyboard.PlaylistsButton(playlists);

        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "یه پلیلیست انتخاب کنید",
            replyMarkup: buttons,
            cancellationToken: ct);
    }

    private async Task HandlePlaylistSubmitAsync(int playlistId, ITelegramBotClient bot, long chatId, int messageId,
        SongSubmissionDto data, CancellationToken ct)
    {
        await bot.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: "موزیکتو بفرست تا به پلیلیست اضافش کنم",
            cancellationToken: ct);

        data.Step = MusicSubmissionStep.WaitingForMusic;
        data.TargetPlaylistId = playlistId;
    }

    
    private async Task ShowCategoriesAsync(ITelegramBotClient bot, long chatId, int messageId, CancellationToken ct)
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