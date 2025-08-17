using AutoMapper;
using Nakisa.Application.DTOs.Music;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Mapping;

public class MusicProfile : Profile
{
    public MusicProfile()
    {
        CreateMap<AddMusicDto, Music>();
    }
}