using Nakisa.Domain.Enums;

namespace Nakisa.Application.DTOs;

public class RegisterDto
{
    public long ChatId { get; set; }
    public string? TelegramId { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? PersonChannelLink { get; set; }
    public long Score { get; set; }
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }
    public CaptionIdentifierType CaptionIdentifier { get; set; }
    public ChannelIncludingType ChannelIncluding { get; set; }
    public RegisterStep Step { get; set; }
}