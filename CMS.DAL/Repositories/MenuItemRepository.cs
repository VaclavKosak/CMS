using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Repositories;

public class MenuItemRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<MenuItemEntity, Guid>(contextFactory, mapper), IAppRepository<MenuItemEntity, Guid>
{
    public async Task<IList<MenuItemEntity>> GetAll(Guid parentId)
    {
        await using var context = ContextFactory();
        return await context.Set<MenuItemEntity>()
            .Where(m => m.ParentId == parentId)
            .OrderBy(m => m.Order).ToListAsync();
    }
        
    public override async Task<IList<MenuItemEntity>> GetAll()
    {
        await using var context = ContextFactory();
        return await context.Set<MenuItemEntity>()
            .OrderBy(m => m.Order).ToListAsync();
    }
}