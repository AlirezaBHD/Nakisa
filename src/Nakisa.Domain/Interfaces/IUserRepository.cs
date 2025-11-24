using Nakisa.Domain.Entities;

namespace Nakisa.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<int> GetUserIdByChatId(long chatId);
}