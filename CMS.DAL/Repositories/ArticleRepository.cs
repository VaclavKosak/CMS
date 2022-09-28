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
    public class ArticleRepository : RepositoryBase<ArticleEntity, Guid>, IAppRepository<ArticleEntity, Guid>
    {
        public ArticleRepository(Func<WebDataContext> contextFactory, IMapper mapper) 
            : base(contextFactory, mapper)
        {
        }
        
        public override async Task<IList<ArticleEntity>> GetAll()
        {
            await using var context = _contextFactory();
            return await context.Set<ArticleEntity>().OrderByDescending(o => o.PublicationDateTime).ToListAsync();
        }
        
        public override async Task<ArticleEntity> GetById(Guid id)
        {
            await using var context = _contextFactory();
            return await context.Set<ArticleEntity>().Include(i => i.Category).AsNoTracking().FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }
        
        public async Task<Guid> Update(ArticleEntity entity, IList<CategoryEntity> categoriesList)
        {
            await using var context = _contextFactory();
            
            var entityExists = await context.Article.Include(i => i.Category).FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (entityExists == null) return default;

            for (var i = 0; i < context.Category.Count(); i++)
            {
                var category = context.Category/*.Include(i => i.Article)*/.Skip(i).First();
                if (categoriesList.Any(i => i.Id == category.Id))
                {
                    if (!entityExists.Category.Any(i => i.Id == category.Id))
                    {
                        entityExists.Category.Add(category);
                    }
                }
                else
                {
                    if (entityExists.Category.Any(i => i.Id == category.Id))
                    {
                        var categoryToRemove = entityExists.Category.Single(c => c.Id == category.Id);
                        entityExists.Category.Remove(categoryToRemove);
                    }
                }
            }
            
            context.Set<ArticleEntity>().Update(entityExists);
            await context.SaveChangesAsync();

            return entityExists.Id;
        }
        
        public async Task<ArticleEntity> GetByUrl(string url)
        {
            await using var context = _contextFactory();
            return await context.Set<ArticleEntity>().FirstOrDefaultAsync(entity => entity.Url.Equals(url));
        }
    }
}