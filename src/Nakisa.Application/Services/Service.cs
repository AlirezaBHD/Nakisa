using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class Service<T> : IService<T> where T : class
{
    #region Injections

    private readonly IMapper _mapper;
    protected readonly IRepository<T> Repository;
    protected readonly IQueryable<T> Queryable;

    public Service(IMapper mapper, IRepository<T> repository)
    {
        _mapper = mapper;
        Repository = repository;
        Queryable = repository.GetLimitedQueryable();
    }

    #endregion

    #region Get All Projected Async

    public async Task<IEnumerable<TDto>> GetAllProjectedAsync<TDto>(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? includes = null,
        TrackingBehavior trackingBehavior = TrackingBehavior.Default,
        bool orderByNewest = true,
        IQueryable<T>? query = null)
    {
        query ??= Queryable;

        if (orderByNewest)
        {
            // query = query.OrderByNewest();//TODO
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        switch (trackingBehavior)
        {
            case TrackingBehavior.AsNoTracking:
                query = query.AsNoTracking();
                break;
            case TrackingBehavior.AsNoTrackingWithIdentityResolution:
                query = query.AsNoTrackingWithIdentityResolution();
                break;
            case TrackingBehavior.Default:
            default:
                break;
        }

        var result = await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
        return result;
    }

    #endregion

    #region Get By Id Projecte dAsync

    public async Task<TDto> GetByIdProjectedAsync<TDto>(
        int id,
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? includes = null,
        TrackingBehavior trackingBehavior = TrackingBehavior.Default,
        IQueryable<T>? query = null)
    {
        query ??= Queryable;

        if (includes != null)
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        switch (trackingBehavior)
        {
            case TrackingBehavior.AsNoTracking:
                query = query.AsNoTracking();
                break;
            case TrackingBehavior.AsNoTrackingWithIdentityResolution:
                query = query.AsNoTrackingWithIdentityResolution();
                break;
            case TrackingBehavior.Default:
            default:
                break;
        }

        var obj = await query.ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => EF.Property<int>(e!, "Id") == id);
        
        if (obj == null)
            // throw new NotFoundException(_displayName); //TODO
            throw new KeyNotFoundException();
        
        return obj;
    }

    #endregion
    
    #region Get All Async

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? includes = null,
        IQueryable<T>? query = null)
    {
        query ??= Queryable;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        

        var result = await query.ToListAsync();
        return result;
    }

    #endregion
}