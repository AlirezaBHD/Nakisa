namespace Nakisa.Application.DTOs.Playlist;

public class ParentPlaylistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Emoji { get; set; }
    public long TelegramChannelId { get; set; }
}