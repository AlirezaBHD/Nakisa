using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Nakisa.Domain.Enums;

namespace Nakisa.Domain.Entities;

[DisplayName("پلی لیست")]
public class Playlist : BaseEntity
{
    [MaxLength(100)]
    [Display(Name = "نام")]
    public required string Name { get; set; }
    
    [MaxLength(10)]
    [Display(Name = "ایموجی")]
    public required string Emoji { get; set; }
    
    public required Category Category { get; set;}
    public int CategoryId { get; set;}
    public long TelegramChannelId { get; set; }
    public required string ChannelInviteLink { get; set; }
    public long TelegramGroupId { get; set; }
    public bool IncludeInMainPage { get; set; }
    public bool IsActive { get; set; }
    public string? ProfileImagePath { get; set; }
    public VisibilityType Visibility { get; set; }
    public ICollection<Music> Musics { get; set; } = new List<Music>();
}