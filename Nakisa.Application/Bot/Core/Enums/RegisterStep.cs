namespace Nakisa.Application.Bot.Core.Enums;

public enum RegisterStep
{
    None,
    ChooseIdentity,
    ChooseLinkType,
    ChoosingNickname,
    SendingChannelLink,
    ChannelPrefix,
    ChannelLinkPrefix,
    Completed
}