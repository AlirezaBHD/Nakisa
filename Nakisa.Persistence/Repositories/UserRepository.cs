using Microsoft.EntityFrameworkCore;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    #region Injection

    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    #endregion

    public async Task<int> GetUserIdByChatId(long chatId)
    {
        var userId = await _context.Users.Where(u => u.ChatId == chatId).Select(u => u.Id).FirstOrDefaultAsync();
        return userId;
    }
}