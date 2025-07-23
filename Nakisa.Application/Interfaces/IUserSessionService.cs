using Nakisa.Domain.Enums;

namespace Nakisa.Application.Interfaces;

public interface IUserSessionService
{
    UserSession GetOrCreate(long chatId);
    void Update(UserSession session);
    void Clear(long chatId);
}