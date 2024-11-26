using AutoMapper;
using CMS.DAL.Entities;
using CMS.Models.User;

namespace CMS.BL.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserModel, AppUser>();
        CreateMap<AppUser, UserModel>();
    }
}