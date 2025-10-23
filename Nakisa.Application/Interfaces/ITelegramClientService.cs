namespace Nakisa.Application.Interfaces;

public interface ITelegramClientService
{
    Task<string?> GetChannelInfoFromLinkAsync(string link);

    Task<(long channelId, long groupId, string channelInvite, string groupInvite)>
        CreateChannelAndGroupAsync(string baseName);

    Task StartLoginAsync();

    void ProvideCode(string code);

    void ProvidePassword(string password);

    bool IsAuthenticated();
}