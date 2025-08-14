using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nakisa.Application.Bot;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.MusicSubmission;
using Nakisa.Application.Bot.MusicSubmission.Steps;
using Nakisa.Application.Bot.PlaylistBrowse;
using Nakisa.Application.Bot.PlaylistBrowse.Steps;
using Nakisa.Application.Bot.Register;
using Nakisa.Application.Bot.Register.Steps;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.Interfaces;
using Nakisa.Application.Services;
using Nakisa.Contracts.Bot;
using Nakisa.Domain.Interfaces;
using Nakisa.Infrastructure.Bot;
using Nakisa.Infrastructure.BotClient;
using Nakisa.Persistence;
using Nakisa.Persistence.Repositories;

namespace Nakisa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<BotService>();
        
        services.AddScoped<IBotFlowDispatcher, BotFlowDispatcher>();
        
        services.AddScoped<IPlaylistBrowseFlowHandler, PlaylistBrowseFlowHandler>();
        
        services.AddScoped<IMusicSubmitFlowHandler,MusicSubmissionFlowHandler>();
        
        services.AddSingleton<IUserSessionService, UserSessionService>();
        
        services.AddScoped<IRegisterFlowHandler, RegisterFlowHandler>();
        
        services.AddScoped<IBotNavigationService, BotNavigationService>();
        
        services.AddScoped<IRegisterStepHandler,ChannelLinkPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChannelPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseIdentityStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseLinkTypeStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChoosingNicknameStepHandler>();
        services.AddScoped<IRegisterStepHandler,SendingChannelLinkStepHandler>();
        
        services.AddScoped<IMusicSubmissionStepHandler,SelectingPlaylistStepHandler>();
        services.AddScoped<IMusicSubmissionStepHandler,WaitingForMusicStepHandler>();
        
        services.AddScoped<IPlaylistBrowseStepHandler,BrowsePlaylistStepHandler>();
        
        services.AddScoped(typeof(IService<>), typeof(Service<>));
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddScoped<IPlaylistService,PlaylistService>();
        services.AddScoped<IMusicService,MusicService>();
        
        services.Configure<TelegramClientOptions>(configuration.GetSection("TelegramClient"));
        services.AddSingleton<ITelegramClientService, TelegramClientService>();
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                )
                .UseSnakeCaseNamingConvention());
        
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<ICategoryRepository,CategoryRepository>();
        services.AddScoped<IPlaylistRepository,PlaylistRepository>();
        services.AddScoped<IMusicRepository,MusicRepository>();
        
        return services;
    }
}
