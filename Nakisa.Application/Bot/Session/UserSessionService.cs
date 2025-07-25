using Nakisa.Application.Bot.Interfaces;
using Nakisa.Domain.Enums;

namespace Nakisa.Application.Bot.Session;

public class UserSessionService : IUserSessionService
{
    private readonly Dictionary<long, UserSession> _sessions = new();

    public UserSession GetOrCreate(long chatId)
    {
        if (!_sessions.TryGetValue(chatId, out var session))
        {
            session = new UserSession { ChatId = chatId, Flow = UserFlow.None };
            _sessions[chatId] = session;
        }

        return session;
    }

    public void Update(UserSession session) => _sessions[session.ChatId] = session;

    public void Clear(long chatId) => _sessions.Remove(chatId);
}