using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Reporitories
{
    public class MenuItemRepository : RepositoryBase<MenuItemEntity, Guid>, IAppRepository<MenuItemEntity, Guid>
    {
        public MenuItemRepository(Func<WebDataContext> contextFactory, IMapper mapper)
            : base(contextFactory, mapper)
        {
        }
        
        public async Task<IList<MenuItemEntity>> GetAll(Guid parentId)
        {
            await using var context = _contextFactory();
            return await context.Set<MenuItemEntity>()
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Order).ToListAsync();
        }
        
        public override async Task<IList<MenuItemEntity>> GetAll()
        {
            await using var context = _contextFactory();
            return await context.Set<MenuItemEntity>()
                .OrderBy(m => m.Order).ToListAsync();
        }
    }
}