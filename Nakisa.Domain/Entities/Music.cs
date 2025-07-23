namespace Nakisa.Domain.Entities;

public class Music : BaseEntity
{
    public required string FileId { get; set; }
    public string? Name { get; set; }
    public string? Artist { get; set; }
    public required User PostedBy { get; set; }
    public int UserId { get; set; }
    public required Playlist Playlist { get; set; }
    public int PlaylistId { get; set; }
}