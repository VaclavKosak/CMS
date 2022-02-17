using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;

namespace CMS.DAL.Repositories
{
    public class UserRepository : RepositoryBase<AppUser, Guid>, IAppRepository<AppUser, Guid>
    {
        public UserRepository(Func<WebDataContext> contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
        }
    }
}