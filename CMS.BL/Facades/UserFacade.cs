using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories;
using CMS.Models.User;

namespace CMS.BL.Facades
{
    public class UserFacade : FacadeBase<UserModel, UserModel, UserModel, UserModel, UserRepository, AppUser, Guid>
    {
        public UserFacade(UserRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}