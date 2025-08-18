using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Domain.Enums;

namespace Nakisa.Application.DTOs;

public class RegisterDto
{
    public long ChatId { get; set; }
    public long? TelegramId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Nickname { get; set; }
    public string? PersonChannelLink { get; set; }
    public CaptionIdentifierType CaptionIdentifier { get; set; }
    public ChannelIncludingType ChannelIncluding { get; set; }
    public RegisterStep Step { get; set; }
}