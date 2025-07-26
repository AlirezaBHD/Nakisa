using Microsoft.EntityFrameworkCore;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Persistence.Repositories;


public class Repository<T> : IRepository<T> where T : class
{
    #region Injections

    private readonly AppDbContext _context;
    private readonly DbSet<T> _entities;
    protected virtual IQueryable<T> LimitedQuery => _entities.AsQueryable();

    public Repository(AppDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    #endregion

    #region Add Async

    public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

    #endregion

    #region Remove

    public void Remove(T entity) => _entities.Remove(entity);

    #endregion

    #region Update

    public void Update(T entity) => _entities.Update(entity);

    #endregion

    #region Save Async

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    #endregion

    #region Get By Id Async

    public async Task<T> GetByIdAsync(int id)
    {
        var obj = await LimitedQuery.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        return obj!;
    }

    #endregion

    #region Get Queryable

    public IQueryable<T> GetQueryable()
    {
        var query = _context.Set<T>().AsQueryable();
        return query;
    }

    #endregion
        
    #region Get Limited IQuaryable

    public IQueryable<T> GetLimitedQueryable()
    {
        return LimitedQuery;
    }

    #endregion
        
}