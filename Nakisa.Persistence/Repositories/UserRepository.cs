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
}