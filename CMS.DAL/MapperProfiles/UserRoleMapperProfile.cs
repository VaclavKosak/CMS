using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles
{
    public class UserRoleMapperProfile : Profile
    {
        public UserRoleMapperProfile()
        {
            CreateMap<AppUserRole, AppUserRole>();
        }
    }
}