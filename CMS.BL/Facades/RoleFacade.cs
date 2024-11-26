using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Role;

namespace CMS.BL.Facades;

public class RoleFacade(RoleRepository repository, IMapper mapper)
    : FacadeBase<RoleModel, RoleModel, RoleModel, RoleModel, RoleRepository, AppRole, Guid>(repository, mapper);