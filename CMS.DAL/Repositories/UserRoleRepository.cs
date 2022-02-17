using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Repositories
{
    public class UserRoleRepository : RepositoryBase<AppUserRole, Guid>, IAppRepository<AppUserRole, Guid>
    {
        public UserRoleRepository(Func<WebDataContext> contextFactory, IMapper mapper) : base(contextFactory, mapper)
        {
        }
        
        public override async Task<IList<AppUserRole>> GetAll()
        {
            await using var context = _contextFactory();
            var items = await context.UserRoles
                .Include(i => i.Role)
                .Include(i => i.User)
                .ToListAsync();
            return items;
        }
        
        public override async Task<AppUserRole> GetById(Guid id)
        {
            await using var context = _contextFactory();
            var item = await context.UserRoles
                .Include(i => i.Role)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.UserId == id);

            return item;
        }
    }
}