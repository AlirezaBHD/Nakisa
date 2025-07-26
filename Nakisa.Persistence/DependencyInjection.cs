using Microsoft.Extensions.DependencyInjection;
using Nakisa.Application.Bot;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.PlaylistBrowse;
using Nakisa.Application.Bot.Register;
using Nakisa.Application.Bot.Register.Steps;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.Bot.SongSubmit;
using Nakisa.Domain.Interfaces;
using Nakisa.Persistence.Repositories;

namespace Nakisa.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository,UserRepository>();
        return services;
    }
}
