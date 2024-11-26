using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles;

public class RoleMapperProfile : Profile
{
    public RoleMapperProfile()
    {
        CreateMap<AppRole, AppRole>();
    }
}