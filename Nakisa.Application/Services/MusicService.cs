using AutoMapper;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class MusicService : Service<Music>, IMusicService
{
    #region Injection

    private readonly IMapper _mapper;

    public MusicService(IMapper mapper, IMusicRepository repository)
        : base(mapper, repository)
    {
        _mapper = mapper;
    }

    #endregion
}