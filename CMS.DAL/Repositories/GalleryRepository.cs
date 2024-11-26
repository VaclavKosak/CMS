using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Repositories;

public class GalleryRepository(Func<WebDataContext> contextFactory, IMapper mapper)
    : RepositoryBase<GalleryEntity, Guid>(contextFactory, mapper), IAppRepository<GalleryEntity, Guid>
{
    public async Task<IList<GalleryEntity>> GetAll(Guid parentId)
    {
        await using var context = ContextFactory();
        return await context.Set<GalleryEntity>().Where(m => m.ParentId == parentId).OrderBy(o => o.DateTime)
            .ToListAsync();
    }

    public async Task<GalleryEntity> GetByUrl(string url, Guid? parentId)
    {
        await using var context = ContextFactory();
        var query = context.Set<GalleryEntity>().AsQueryable();
        if (parentId != null) query = query.Where(m => m.ParentId == parentId.Value);
        return await query.FirstOrDefaultAsync(entity => entity.Url.Equals(url));
    }
}