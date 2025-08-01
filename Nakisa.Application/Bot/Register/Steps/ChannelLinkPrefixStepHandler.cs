using Nakisa.Application.Bot.Extensions;
using Nakisa.Application.Bot.Interfaces;
using Nakisa.Application.DTOs;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Nakisa.Application.Bot.Register.Steps;


public class ChannelLinkPrefixStepHandler : IRegisterStepHandler
{
    private readonly IUserService _userService;
    private readonly IBotNavigationService _navigation;
    public ChannelLinkPrefixStepHandler(IUserService userService, IBotNavigationService navigation)
    {
        _userService = userService;
        _navigation = navigation;
    }
    public RegisterStep Step => RegisterStep.ChannelLinkPrefix;

    public async Task HandleAsync(Update update, RegisterDto data, ITelegramBotClient bot, CancellationToken ct)
    {
        var chatId = update.GetChatId();
        var channelLink = update.Message!.Text;
        
        if (channelLink.TryNormalizeTelegramChannelLink(out var cleanLink, publicOnly: true))
        {
            data.PersonChannelLink = cleanLink;

            await _userService.AddOrUpdate(data);
        
            data.Step = RegisterStep.Completed;
            await bot.SendMessage(
                chatId: chatId,
                text: "ثبت نام موفق",
                cancellationToken: ct);

            await _navigation.SendHomePageAsync(bot, chatId, ct);
        }
        else
        {
            await bot.SendMessage(
                chatId: chatId,
                text: "لینک ارسال شده اشتباه است \nفرمت صحیح:\n@TheOsservatore\nt.me/TheOsservatore\nhttps://t.me/TheOsservatore",
                cancellationToken: ct);
        }
    }
}