using Microsoft.Extensions.DependencyInjection;
using Nakisa.Application.Bot;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.MusicSubmission;
using Nakisa.Application.Bot.MusicSubmission.Steps;
using Nakisa.Application.Bot.PlaylistBrowse;
using Nakisa.Application.Bot.Register;
using Nakisa.Application.Bot.Register.Steps;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.Interfaces;
using Nakisa.Application.Services;
using Nakisa.Infrastructure.Bot;

namespace Nakisa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHostedService<BotService>();
        
        services.AddScoped<IBotFlowDispatcher, BotFlowDispatcher>();
        
        services.AddScoped<IPlaylistBrowseFlowHandler, PlaylistBrowseFlowHandler>();
        
        services.AddScoped<IMusicSubmitFlowHandler,MusicSubmissionFlowHandler>();
        
        services.AddSingleton<IUserSessionService, UserSessionService>();
        
        services.AddScoped<IRegisterFlowHandler, RegisterFlowHandler>();
        
        services.AddScoped<IRegisterStepHandler,ChannelLinkPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChannelPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseIdentityStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseLinkTypeStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChoosingNicknameStepHandler>();
        services.AddScoped<IRegisterStepHandler,SendingChannelLinkStepHandler>();
        
        services.AddScoped<IMusicSubmissionStepHandler,SelectingPlaylistStepHandler>();
        services.AddScoped<IMusicSubmissionStepHandler,WaitingForMusicStepHandler>();
        
        services.AddScoped(typeof(IService<>), typeof(Service<>));
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<ICategoryService,CategoryService>();
        services.AddScoped<IPlaylistService,PlaylistService>();
        services.AddScoped<IMusicService,MusicService>();

        return services;
    }
}
