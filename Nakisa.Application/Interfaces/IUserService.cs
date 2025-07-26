using Nakisa.Application.DTOs;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Interfaces;

public interface IUserService : IService<User>
{
    Task AddOrUpdate(RegisterDto registerDto);
    Task<bool> IsNicknameTaken(string nickname);
}