namespace Nakisa.Domain.Enums;

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