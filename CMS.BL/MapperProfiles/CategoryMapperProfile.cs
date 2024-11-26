using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Category;

namespace CMS.BL.MapperProfiles;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<CategoryEntity, CategoryModel>();
        CreateMap<CategoryModel, CategoryEntity>();
        CreateMap<CategoryModel, CategoryModel>();
    }
}