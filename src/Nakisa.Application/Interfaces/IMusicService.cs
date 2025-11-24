using Nakisa.Domain.Entities;
using Telegram.Bot.Types;

namespace Nakisa.Application.Interfaces;

public interface IMusicService : IService<Music>
{
    Task AddMusicAsync(Audio audio, long chatId, int playlistId);
}