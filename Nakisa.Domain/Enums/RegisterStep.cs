namespace Nakisa.Domain.Enums;

public enum RegisterStep
{
    None,
    AwaitingName,
    AwaitingEmail,
    AwaitingPhone,
    Completed
}