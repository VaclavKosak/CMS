using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.Role;

namespace CMS.BL.MapperProfiles;

public class RoleMapperProfile : Profile
{
    public RoleMapperProfile()
    {
        CreateMap<RoleModel, AppRole>();
        CreateMap<AppRole, RoleModel>();
    }
}