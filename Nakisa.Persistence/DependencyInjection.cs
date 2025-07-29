using Microsoft.Extensions.DependencyInjection;
using Nakisa.Domain.Interfaces;
using Nakisa.Persistence.Repositories;

namespace Nakisa.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<ICategoryRepository,CategoryRepository>();
        services.AddScoped<IPlaylistRepository,PlaylistRepository>();
        services.AddScoped<IMusicRepository,MusicRepository>();
        return services;
    }
}
