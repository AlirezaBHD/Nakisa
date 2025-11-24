using Nakisa.Application.Bot.Core.Enums;

namespace Nakisa.Application.DTOs;

public class PlaylistBrowseDto
{
    public PlaylistBrowseStep Step { get; set; }
    public int TargetPlaylistId { get; set; }
}