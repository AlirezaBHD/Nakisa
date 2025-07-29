using Nakisa.Application.DTOs.Playlist;

namespace Nakisa.Application.DTOs.Category;

public class GetCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MainPagePlaylistsDto> Playlists { get; set; }
}