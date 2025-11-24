using AutoMapper;
using Nakisa.Application.DTOs;
using Nakisa.Application.DTOs.User;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
    }
}