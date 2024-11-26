using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.User;

namespace CMS.BL.Facades;

public class UserFacade(UserRepository repository, IMapper mapper)
    : FacadeBase<UserModel, UserModel, UserModel, UserModel, UserRepository, AppUser, Guid>(repository, mapper);