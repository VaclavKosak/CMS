using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CMS.DAL.Reporitories
{
    public class RoleRepository : RepositoryBase<AppRole, Guid>, IAppRepository<AppRole, Guid>
    {
        public RoleRepository(Func<WebDataContext> contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
        }
    }
}