using Nakisa.Application.DTOs.Playlist;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Interfaces;

public interface IPlaylistService : IService<Playlist>
{
    Task<IEnumerable<MainPagePlaylistsDto>> GetPlaylistsByCategoryId(int categoryId);
    
    Task<List<MainPagePlaylistsDto>> GetPlaylistsByParentId(int playlistId);

}