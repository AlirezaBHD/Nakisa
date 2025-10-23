using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Nakisa.Application.Interfaces;
using Nakisa.Contracts.Bot;
using TL;
using WTelegram;

namespace Nakisa.Infrastructure.BotClient;

public class TelegramClientService : ITelegramClientService, IAsyncDisposable
{
    private readonly TelegramClientOptions _options;
    private readonly Client _client;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _waiters = new();

    public TelegramClientService(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
        _client = new Client(Config);
    }
    
    private string? Config(string what) => what switch
    {
        "api_id"            => _options.ApiId.ToString(),
        "api_hash"          => _options.ApiHash,
        "phone_number"      => _options.PhoneNumber,
        "session_pathname"  => _options.SessionPath,
        "verification_code" => WaitForValue("code", TimeSpan.FromMinutes(5)),
        "password" => WaitForValue("password", TimeSpan.FromMinutes(5)),
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
    
    public string WaitForValue(string key, TimeSpan timeout)
    {
        var tcs = _waiters.GetOrAdd(key, _ => new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously));
        try
        {
            if (tcs.Task.Wait(timeout)) return tcs.Task.Result;
            throw new TimeoutException($"Timed out waiting for {key}");
        }
        finally
        {
            _waiters.TryRemove(key, out var _);
        }
    }

    public async Task StartLoginAsync()
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await _client.ConnectAsync();
                var user = await _client.LoginUserIfNeeded();
                Console.WriteLine($"Logged in as {user}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login failed: " + ex);
            }
        });
    }

    public void ProvideCode(string code)
    {
        if (_waiters.TryGetValue("code", out var tcs))
            tcs.TrySetResult(code);
        else
        {
            var newtcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            newtcs.TrySetResult(code);
            _waiters["code"] = newtcs;
        }
    }

    public void ProvidePassword(string password)
    {
        if (_waiters.TryGetValue("password", out var tcs))
            tcs.TrySetResult(password);
        else
        {
            var newtcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            newtcs.TrySetResult(password);
            _waiters["password"] = newtcs;
        }
    }

    public bool IsAuthenticated() => _client.User != null;
}