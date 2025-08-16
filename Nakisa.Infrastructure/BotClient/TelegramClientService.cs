using Microsoft.Extensions.Options;
using Nakisa.Application.Interfaces;
using Nakisa.Contracts.Bot;
using TL;

namespace Nakisa.Infrastructure.BotClient;

public class TelegramClientService : ITelegramClientService, IAsyncDisposable
{
    private readonly TelegramClientOptions _options;
    private readonly WTelegram.Client _client;

    public TelegramClientService(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
        _client = new WTelegram.Client(Config);
    }

    private string? Config(string what) => what switch
    {
        "api_id" => _options.ApiId.ToString(),
        "api_hash" => _options.ApiHash,
        "phone_number" => _options.PhoneNumber,
        _ => null
    };
    
    public async Task<string?> GetChannelInfoFromLinkAsync(string link)
    {
        if (string.IsNullOrWhiteSpace(link))
            return null;
        
        await _client.LoginUserIfNeeded();

        if (link.Contains("+")) //private link
        {
            var inviteHash = link.Split('+').Last();
            var invite = await _client.Messages_CheckChatInvite(inviteHash);

            switch (invite)
            {
                case ChatInvite chatInvite:
                    return chatInvite.title;
                
                case ChatInviteAlready chatAlready:
                    return chatAlready.chat.Title;
            }
        }
        else
        {
            var username = link.Split('/').Last();
            var resolved = await _client.Contacts_ResolveUsername(username);

            if (resolved.chats.Values.FirstOrDefault() is Channel channel)
            {
                return channel.Title;
            }
        }
        return null;
    }

    public async Task<(long channelId, long groupId, string channelInvite, string groupInvite)>
        CreateChannelAndGroupAsync(string baseName)
    {
        await _client.LoginUserIfNeeded();

        var channelResult = await _client.Channels_CreateChannel(
            broadcast: true,
            title: baseName,
            about: baseName + " playlist\n@NakisaMusicBot",
            megagroup: false);

        var channel = channelResult.Chats.First().Value as Channel;

        var groupResult = await _client.Channels_CreateChannel(
            megagroup: true,
            title: baseName + " chat",
            about: baseName + " playlist chat\n@NakisaMusicBot"
            );

        var group = groupResult.Chats.First().Value as Channel;

        await _client.Channels_SetDiscussionGroup(channel, group);

        var channelInvite = await _client.Messages_ExportChatInvite(channel, legacy_revoke_permanent: true);
        var groupInvite = await _client.Messages_ExportChatInvite(group, legacy_revoke_permanent: true);
        
        
        return (channel.id, group.id,
            (channelInvite as ChatInviteExported)?.link ?? "",
            (groupInvite as ChatInviteExported)?.link ?? "");
    }
    
    public ValueTask DisposeAsync() => _client.DisposeAsync();
}