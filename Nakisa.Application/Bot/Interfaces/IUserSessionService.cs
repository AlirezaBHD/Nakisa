using Nakisa.Application.Bot.Session;

namespace Nakisa.Application.Bot.Interfaces;

public interface IUserSessionService
{
    UserSession GetOrCreate(long chatId);
    void Update(UserSession session);
    void Clear(long chatId);
}