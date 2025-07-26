using AutoMapper;
using Nakisa.Application.DTOs;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}