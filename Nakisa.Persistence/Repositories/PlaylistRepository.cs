using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Persistence.Repositories;

public class PlaylistRepository : Repository<Playlist>, IPlaylistRepository
{
    #region Injection

    private readonly AppDbContext _context;
    public PlaylistRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    #endregion
}