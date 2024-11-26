using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.MenuItem;

namespace CMS.BL.MapperProfiles;

public class MenuItemMapperProfile : Profile
{
    public MenuItemMapperProfile()
    {
        CreateMap<MenuItemEntity, MenuItemListModel>();
        CreateMap<MenuItemNewModel, MenuItemEntity>();
        CreateMap<MenuItemEntity, MenuItemDetailModel>();
        CreateMap<MenuItemDetailModel, MenuItemNewModel>();

        CreateMap<MenuItemUpdateModel, MenuItemEntity>();
        CreateMap<MenuItemDetailModel, MenuItemUpdateModel>();
    }
}