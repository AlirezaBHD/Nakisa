using AutoMapper;
using Nakisa.Application.DTOs.Playlist;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Enums;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class PlaylistService : Service<Playlist>, IPlaylistService
{
    #region Injection

    private readonly IMapper _mapper;
    private readonly ITelegramClientService _telegramBotClient;

    public PlaylistService(IMapper mapper, IPlaylistRepository repository, ITelegramClientService telegramBotClient)
        : base(mapper, repository)
    {
        _mapper = mapper;
        _telegramBotClient = telegramBotClient;
    }

    #endregion

    public async Task<IEnumerable<MainPagePlaylistsDto>> GetPlaylistsByCategoryId(int categoryId)
    {
        var result = await GetAllProjectedAsync<MainPagePlaylistsDto>(
            predicate: p => p.CategoryId == categoryId && p.IsActive && p.SubPlaylists.Any(),
            trackingBehavior: TrackingBehavior.AsNoTracking);

        return result;
    }

    public async Task<List<MainPagePlaylistsDto>> GetPlaylistsByParentId(int playlistId)
    {
        var result = await GetAllProjectedAsync<MainPagePlaylistsDto>(
            predicate: p => (p.ParentId == playlistId || p.Id == playlistId) && p.IsActive,
            trackingBehavior: TrackingBehavior.AsNoTracking);

        return result.ToList();
    }

    public async Task<IEnumerable<PlaylistsDto>> GetPlaylistsInfo(int playlistId)
    {
        var query = Queryable
            .Where(p => p.Id == playlistId || p.Id == Queryable
                .Where(x => x.Id == playlistId)
                .Select(x => x.ParentId)
                .FirstOrDefault());

        var result = await GetAllProjectedAsync<PlaylistsDto>(query: query,
            includes: [p => p.Parent!],
            trackingBehavior: TrackingBehavior.AsNoTrackingWithIdentityResolution);

        return result;
    }

    public async Task<List<BrowsePlaylistDto>> GetPlaylistsInfoByParentId(int playlistId)
    {
        var result = await GetAllProjectedAsync<BrowsePlaylistDto>(
            predicate: p => (p.ParentId == playlistId || p.Id == playlistId) && p.IsActive,
            trackingBehavior: TrackingBehavior.AsNoTracking);

        return result.ToList();
    }

    public async Task CreateActivePlaylists()
    {
        var activePlaylists = await GetAllAsync(predicate: p => p.IsActive && p.ChannelInviteLink == "https://t.me/unknown");

        foreach (var playlist in activePlaylists)
        {
            var playlistEmoji = playlist.Emoji == null ? "" : $"{playlist.Emoji} ";
            var playlistName = $"{playlistEmoji}{playlist.Name}";
            var result = await _telegramBotClient.CreateChannelAndGroupAsync(playlistName);

            playlist.TelegramChannelId = long.Parse($"-100{result.channelId}");
            playlist.TelegramGroupId = long.Parse($"-100{result.groupId}");
            playlist.ChannelInviteLink = result.channelInvite;
            playlist.GroupInviteLink = result.groupInvite;

            Repository.Update(playlist);
            await Repository.SaveAsync();
        }
    }
}