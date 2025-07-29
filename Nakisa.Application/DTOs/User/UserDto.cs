using Nakisa.Domain.Enums;

namespace Nakisa.Application.DTOs.User;

public class UserDto
{
    public required long ChatId { get; set; }
    public long? TelegramId { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? PersonChannelLink { get; set; }
    public CaptionIdentifierType CaptionIdentifier { get; set; }
    public ChannelIncludingType ChannelIncluding { get; set; }
}