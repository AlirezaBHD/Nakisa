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

    public PlaylistService(IMapper mapper, IPlaylistRepository repository)
        : base(mapper, repository)
    {
        _mapper = mapper;
    }

    #endregion

    public async Task<IEnumerable<MainPagePlaylistsDto>> GetPlaylistsByCategoryId(int categoryId)
    {
        var result = await GetAllProjectedAsync<MainPagePlaylistsDto>(
            predicate: p => p.CategoryId == categoryId && p.ParentId == null,
            trackingBehavior: TrackingBehavior.AsNoTracking);

        return result;
    }
    
    public async Task<List<MainPagePlaylistsDto>> GetPlaylistsByParentId(int playlistId)
    {
        var result = await GetAllProjectedAsync<MainPagePlaylistsDto>(
            predicate: p => p.ParentId == playlistId || p.Id == playlistId,
            trackingBehavior: TrackingBehavior.AsNoTracking);
        
        return result.ToList();
    }
}