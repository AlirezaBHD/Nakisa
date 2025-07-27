using AutoMapper;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class PlaylistService : Service<Playlist>, IPlaylistService
{
    #region Injection

    private readonly IMapper _mapper;

    public PlaylistService(IMapper mapper, IPlaylistRepository repository)
        : base(mapper, repository)
    {
        _mapper = mapper;
    }

    #endregion
}