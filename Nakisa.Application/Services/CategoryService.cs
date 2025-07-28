using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nakisa.Application.DTOs.Category;
using Nakisa.Application.Interfaces;
using Nakisa.Domain.Entities;
using Nakisa.Domain.Enums;
using Nakisa.Domain.Interfaces;

namespace Nakisa.Application.Services;

public class CategoryService : Service<Category>, ICategoryService
{
    #region Injection

    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, ICategoryRepository repository)
        : base(mapper, repository)
    {
        _mapper = mapper;
    }

    #endregion
}