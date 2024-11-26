using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.UserRole;

namespace CMS.BL.MapperProfiles;

public class UserRoleMapperProfile : Profile
{
    public UserRoleMapperProfile()
    {
        CreateMap<UserRoleModel, AppUserRole>();
        CreateMap<AppUserRole, UserRoleModel>();
    }
}