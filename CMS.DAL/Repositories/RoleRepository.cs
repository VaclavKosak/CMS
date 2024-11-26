using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;

namespace CMS.DAL.Repositories;

public class RoleRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<AppRole, Guid>(contextFactory, mapper), IAppRepository<AppRole, Guid>;