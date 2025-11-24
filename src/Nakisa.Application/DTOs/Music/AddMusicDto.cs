namespace Nakisa.Application.DTOs.Music;

public class AddMusicDto
{
    public string FileId { get; set; }
    public string? Name { get; set; }
    public string? Artist { get; set; }
    public int UserId { get; set; }
    public int PlaylistId { get; set; }
}