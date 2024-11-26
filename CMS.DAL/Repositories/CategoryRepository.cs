using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Repositories;

public class CategoryRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<CategoryEntity, Guid>(contextFactory, mapper), IAppRepository<CategoryEntity, Guid>
{
    public virtual async Task<IList<CategoryEntity>> GetAllByIds(Guid[] ids)
    {
        await using var context = ContextFactory();
        return await context.Set<CategoryEntity>().Where(w => ids.Contains(w.Id)).Include(i => i.Article).ToListAsync();
    }
        
    public override async Task<CategoryEntity> GetById(Guid id)
    {
        await using var context = ContextFactory();
        return await context.Set<CategoryEntity>().Include(i => i.Article).FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }
}