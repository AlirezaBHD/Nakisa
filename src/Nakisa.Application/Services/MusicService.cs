using AutoMapper;
using Nakisa.Application.DTOs.Music;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;
using Telegram.Bot.Types;

namespace Nakisa.Application.Services;

public class MusicService : Service<Music>, IMusicService
{
    #region Injection

    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public MusicService(IMapper mapper, IMusicRepository repository, IUserRepository userRepository)
        : base(mapper, repository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    #endregion

    public async Task AddMusicAsync(Audio audio, long chatId, int playlistId)
    {
        var userId = await _userRepository.GetUserIdByChatId(chatId);
        var music = new AddMusicDto
        {
            FileId = audio.FileId,
            Artist = audio.Performer,
            Name = audio.Title,
            UserId = userId,
            PlaylistId = playlistId
        };

        var musicEntity = _mapper.Map<Music>(music);
        
        await Repository.AddAsync(musicEntity);
        await Repository.SaveAsync();
    }
}