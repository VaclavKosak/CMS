using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories.Interfaces;

namespace CMS.DAL.Reporitories
{
    public class UserRepository : RepositoryBase<AppUser, Guid>, IAppRepository<AppUser, Guid>
    {
        public UserRepository(Func<WebDataContext> contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
        }
    }
}