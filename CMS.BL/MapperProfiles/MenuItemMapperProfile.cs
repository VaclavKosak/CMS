using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.MenuItem;

namespace CMS.BL.MapperProfiles;

public class MenuItemMapperProfile : Profile
{
    public MenuItemMapperProfile()
    {
        CreateMap<MenuItemEntity, MenuItemModel>();
        CreateMap<MenuItemModel, MenuItemEntity>();
        CreateMap<MenuItemModel, MenuItemModel>();
    }
}