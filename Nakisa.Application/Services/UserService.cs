using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nakisa.Application.DTOs;
using Nakisa.Application.DTOs.User;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Enums;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class UserService : Service<User>, IUserService
{
    #region Injection

    private readonly IMapper _mapper;
    private readonly ITelegramClientService _client;

    public UserService(IMapper mapper, IUserRepository repository, ITelegramClientService client)
        : base(mapper, repository)
    {
        _mapper = mapper;
        _client = client;
    }

    #endregion

    public async Task AddOrUpdate(RegisterDto registerDto)
    {
        var entity = await Repository.GetQueryable().FirstOrDefaultAsync(x => x.ChatId == registerDto.ChatId);

        if (entity == null)
        {
            await CreateUserAsync(registerDto);
        }
        else
        {
            await UpdateUserAsync(registerDto, entity);
        }
    }

    private async Task CreateUserAsync(RegisterDto registerDto)
    {
        var entity = _mapper.Map<User>(registerDto);
        await Repository.AddAsync(entity);
        await Repository.SaveAsync();
    }

    private async Task UpdateUserAsync(RegisterDto registerDto, User entity)
    {
        entity = _mapper.Map(registerDto, entity);
        Repository.Update(entity);
        await Repository.SaveAsync();
    }

    public async Task<bool> IsNicknameTaken(string nickname, long chatId)
    {
        var queryable = Repository.GetQueryable();
        var result = await queryable.AnyAsync(u => u.Nickname == nickname && u.ChatId != chatId);

        return result;
    }

    public async Task<bool> IsUserExist(long chatId)
    {
        var queryable = Repository.GetQueryable();
        var isExist = await queryable.AnyAsync(u => u.ChatId == chatId);
        return isExist;
    }

    public async Task<string> GenerateCaption(long chatId)
    {
        var query = await GetAllProjectedAsync<UserDto>(
            predicate: u => u.ChatId == chatId,
            trackingBehavior: TrackingBehavior.AsNoTracking);

        var user = query.First();

        var captionIdentifier = user.CaptionIdentifier;

        var channelIncluding = user.ChannelIncluding;

        var identifier = string.Empty;
        switch (captionIdentifier)
        {
            case CaptionIdentifierType.TelegramName:

                identifier = user.FirstName;
                break;

            case CaptionIdentifierType.Nickname:
                identifier = user.Nickname!;
                break;
            case CaptionIdentifierType.Unknown:
                identifier = "ناشناس";
                break;
            case CaptionIdentifierType.TelegramNameAndUsername:
                identifier = $"<a href=\"https://t.me/{user.Username}\">{user.FirstName!}</a>";
                break;
            case CaptionIdentifierType.TelegramNameAndChannelName:
                identifier = $"<a href=\"{user.PersonChannelLink}\">{user.FirstName!}</a>";
                break;

            case CaptionIdentifierType.NicknameAndUsername:
                identifier = $"({user.Nickname!})[@{user.Username}]";
                break;
            case CaptionIdentifierType.NicknameAndChannelName:
                identifier = $"<a href=\"{user.PersonChannelLink}\">{user.Nickname!}</a>";
                break;
        }

        var includedChannel = string.Empty;
        switch (channelIncluding)
        {
            case ChannelIncludingType.None:
                break;

            case ChannelIncludingType.ChannelName:
                includedChannel = await _client.GetChannelInfoFromLinkAsync(user.PersonChannelLink!);
                break;

            case ChannelIncludingType.ChannelNameWithLink:
                var channelName = await _client.GetChannelInfoFromLinkAsync(user.PersonChannelLink!);
                includedChannel = $"<a href=\"{user.PersonChannelLink}\">{channelName}</a>";
                break;
        }

        var caption = identifier;

        if (!string.IsNullOrEmpty(includedChannel))
        {
            caption = $"{caption}\n{includedChannel}";
        }

        return caption;
    }
}