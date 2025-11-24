using AutoMapper;
using Nakisa.Application.DTOs.Category;
using Nakisa.Domain.Entities;

namespace Nakisa.Application.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GetCategoryDto>()
            .ForMember(dest => dest.Playlists, opt =>
                opt.MapFrom(src => src.Playlists
                    .Where(p => p.IsActive && p.IncludeInMainPage)));
        
        CreateMap<Category, BrowseCategoryDto>()
            .ForMember(dest => dest.Playlists, opt =>
                opt.MapFrom(src => src.Playlists
                    .Where(p => p.IsActive && p.ParentId == null)));
    }
}