using Nakisa.Domain.Enums;

namespace Nakisa.Domain.Entities;

public class User: BaseEntity
{
    public required long ChatId { get; set; }
    public long? TelegramId { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? PersonChannelLink { get; set; }
    public long Score { get; set; }
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }
    public CaptionIdentifierType CaptionIdentifier { get; set; }
    public ChannelIncludingType ChannelIncluding { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsStaff { get; set; } = false;
    public ICollection<Music> PostedMusics { get; set; } = new List<Music>();
}