using AutoMapper;
using CMS.DAL.Entities;

namespace CMS.DAL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<AppUser, AppUser>();
        }
    }
}