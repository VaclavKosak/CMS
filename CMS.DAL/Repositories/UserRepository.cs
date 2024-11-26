using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;

namespace CMS.DAL.Repositories;

public class UserRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<AppUser, Guid>(contextFactory, mapper), IAppRepository<AppUser, Guid>;