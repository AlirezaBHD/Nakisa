namespace Nakisa.Application.Bot.Extensions;

public static class StringExtensions
{
    public static string ToTelegramLink(this string text, string url)
    {
        return $"<a href=\"{url}\">{text}</a>";
    }
    
    public static string EmojiHandler(this string? emoji)
    {
        return string.IsNullOrWhiteSpace(emoji) ? "" : emoji + " ";
    }
}