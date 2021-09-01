using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories;
using CMS.Models.Role;

namespace CMS.BL.Facades
{
    public class RoleFacade : FacadeBase<RoleModel, RoleModel, RoleModel, RoleModel, RoleRepository, AppRole, Guid>
    {
        public RoleFacade(RoleRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}