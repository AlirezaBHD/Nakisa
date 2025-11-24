using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    #region Injection

    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    #endregion
}