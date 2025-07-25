using Microsoft.Extensions.DependencyInjection;
using Nakisa.Application.Bot;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.Bot.PlaylistBrowse;
using Nakisa.Application.Bot.Register;
using Nakisa.Application.Bot.Register.Steps;
using Nakisa.Application.Bot.Session;
using Nakisa.Application.Bot.SongSubmit;
using Nakisa.Infrastructure.Bot;

namespace Nakisa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHostedService<BotService>();
        
        services.AddScoped<IBotFlowDispatcher, BotFlowDispatcher>();
        
        services.AddScoped<IPlaylistBrowseFlowHandler, PlaylistBrowseFlowHandler>();
        
        services.AddScoped<ISongSubmitFlowHandler,SongSubmitFlowHandler>();
        
        services.AddSingleton<IUserSessionService, UserSessionService>();
        
        services.AddScoped<IRegisterFlowHandler, RegisterFlowHandler>();
        
        services.AddScoped<IRegisterStepHandler,ChannelLinkPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChannelPrefixStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseIdentityStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChooseLinkTypeStepHandler>();
        services.AddScoped<IRegisterStepHandler,ChoosingNicknameStepHandler>();
        services.AddScoped<IRegisterStepHandler,SendingChannelLinkStepHandler>();

        return services;
    }
}
