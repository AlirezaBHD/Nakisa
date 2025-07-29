using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class UserService : Service<User>, IUserService
{
    #region Injection

    private readonly IMapper _mapper;

    public UserService(IMapper mapper, IUserRepository repository)
        : base(mapper, repository)
    {
        _mapper = mapper;
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
            await UpdateUserAsync(registerDto,entity);
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

    public async Task<bool> IsNicknameTaken(string nickname , long chatId)
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
}