using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<CategoryEntity, CategoryEntity>();
    }
}