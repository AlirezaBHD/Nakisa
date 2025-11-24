using Nakisa.Application.DTOs.Playlist;

namespace Nakisa.Application.DTOs.Category;

public class BrowseCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<BrowsePlaylistDto> Playlists { get; set; }
}