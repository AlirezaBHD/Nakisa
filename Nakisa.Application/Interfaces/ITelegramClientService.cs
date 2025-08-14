namespace Nakisa.Application.Interfaces;

public interface ITelegramClientService
{
    Task<string?> GetChannelInfoFromLinkAsync(string link);
}