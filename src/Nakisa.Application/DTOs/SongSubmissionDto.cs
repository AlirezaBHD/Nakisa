using Nakisa.Application.Bot.Core.Enums;

namespace Nakisa.Application.DTOs;

public class SongSubmissionDto
{
    public MusicSubmissionStep Step { get; set; }
    public int TargetPlaylistId { get; set; }
}