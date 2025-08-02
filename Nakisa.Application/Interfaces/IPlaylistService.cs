using Nakisa.Application.DTOs.Playlist;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Interfaces;

public interface IPlaylistService : IService<Playlist>
{
    Task<IEnumerable<MainPagePlaylistsDto>> GetPlaylistsByCategoryId(int categoryId);
    
    Task<List<MainPagePlaylistsDto>> GetPlaylistsByParentId(int playlistId);
    Task<IEnumerable<PlaylistsDto>> GetPlaylistsInfo(int playlistId);
    Task<List<BrowsePlaylistDto>> GetPlaylistsInfoByParentId(int playlistId);
}