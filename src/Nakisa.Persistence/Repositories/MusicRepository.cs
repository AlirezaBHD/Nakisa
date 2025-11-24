using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Persistence.Repositories;

public class MusicRepository : Repository<Music>, IMusicRepository
{
    #region Injection

    private readonly AppDbContext _context;
    public MusicRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    #endregion
}