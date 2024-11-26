using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Category;

namespace CMS.BL.MapperProfiles;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<CategoryEntity, CategoryListModel>();
        CreateMap<CategoryNewModel, CategoryEntity>();
        CreateMap<CategoryEntity, CategoryDetailModel>();
        CreateMap<CategoryDetailModel, CategoryNewModel>();
            
        CreateMap<CategoryUpdateModel, CategoryEntity>();
        CreateMap<CategoryDetailModel, CategoryUpdateModel>();
    }
}