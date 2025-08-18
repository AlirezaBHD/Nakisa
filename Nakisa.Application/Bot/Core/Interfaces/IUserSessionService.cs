using Nakisa.Application.Bot.Core.Session;

namespace Nakisa.Application.Bot.Core.Interfaces;

public interface IUserSessionService
{
    UserSession GetOrCreate(long chatId);
    void Update(UserSession session);
    void Clear(long chatId);
}