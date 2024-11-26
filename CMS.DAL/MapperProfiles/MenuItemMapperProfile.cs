using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles;

public class MenuItemMapperProfile : Profile
{
    public MenuItemMapperProfile()
    {
        CreateMap<MenuItemEntity, MenuItemEntity>();
    }
}