using Nakisa.Application.Bot.Core.Enums;
using Nakisa.Application.Bot.Core.Interfaces;

namespace Nakisa.Application.Bot.Core.Session;

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

    public void Clear(long chatId)
    {
        GetOrCreate(chatId).Flow = UserFlow.None;
        _sessions.Remove(chatId);
    }
}