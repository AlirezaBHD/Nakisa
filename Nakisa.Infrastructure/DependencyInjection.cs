using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nakisa.Application.Bot.Core;
using Nakisa.Application.Bot.Core.Interfaces;
using Nakisa.Application.Bot.Core.Session;
using Nakisa.Application.Bot.Flows.MusicSubmission;
using Nakisa.Application.Bot.Flows.MusicSubmission.Steps;
using Nakisa.Application.Bot.Flows.PlaylistBrowse;
using Nakisa.Application.Bot.Flows.PlaylistBrowse.Steps;
using Nakisa.Application.Bot.Flows.Register;
using Nakisa.Application.Bot.Flows.Register.Steps;
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
        
        services.AddScoped<IFlowHandler, PlaylistBrowseFlowHandler>();
        
        services.AddScoped<IFlowHandler,MusicSubmissionFlowHandler>();
        
        services.AddScoped<IFlowHandler, RegisterFlowHandler>();
        
        services.AddSingleton<IUserSessionService, UserSessionService>();
        
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
        services.AddSingleton<TelegramConfigProvider>();
        
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
        
        services.AddOpenApi();

        services.AddSwaggerGen();
        
        
        return services;
    }
}
