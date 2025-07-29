namespace Nakisa.Application.Bot.Extensions;

public static class TelegramValidationExtensions
{
    public static bool TryNormalizeTelegramChannelLink(this string? channelLink, out string normalizedLink, bool publicOnly = false)
    {
        normalizedLink = string.Empty;

        if (string.IsNullOrWhiteSpace(channelLink))
            return false;

        channelLink = channelLink.Trim();

        if (publicOnly)
        {
            if (channelLink.StartsWith("t.me/+") || channelLink.StartsWith("https://t.me/+"))
                return false;
        }

        if (channelLink.StartsWith("@"))
        {
            normalizedLink = $"https://t.me/{channelLink[1..]}";
            return true;
        }
        else if (channelLink.StartsWith("https://t.me/"))
        {
            normalizedLink = channelLink;
            return true;
        }
        else if (channelLink.StartsWith("t.me/"))
        {
            normalizedLink = $"https://{channelLink}";
            return true;
        }

        return false;
    }
}
