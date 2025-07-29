using AutoMapper;
using Nakisa.Application.DTOs.Playlist;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Mapping;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<Playlist, MainPagePlaylistsDto>();
        CreateMap<Playlist, PlaylistsDto>();
        CreateMap<Playlist, ParentPlaylistDto>();
    }
}