using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CMS.DAL.Repositories
{
    public class RepositoryBase<TEntity, TId> 
        where TEntity : class, IEntity<TId>
    {
        protected readonly Func<WebDataContext> _contextFactory;
        protected readonly IMapper _mapper;

        public RepositoryBase(Func<WebDataContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }
        
        public virtual async Task<IList<TEntity>> GetAll()
        {
            await using var context = _contextFactory();
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(TId id)
        {
            await using var context = _contextFactory();
            return await context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public virtual async Task<TId> Insert(TEntity entity)
        {
            await using var context = _contextFactory();

            if (entity is IEntity<Guid> guidEntity)
            {
                guidEntity.Id = Guid.NewGuid();
            }
            else
            {
                entity.Id = default;
            }
            
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }
        
        public virtual async Task<TId> Update(TEntity entity)
        {
            await using var context = _contextFactory();
            
            var entityExists = await GetById(entity.Id);
            if (entityExists == null) return default;
            _mapper.Map(entity, entityExists);
            context.Set<TEntity>().Update(entityExists);
            await context.SaveChangesAsync();

            return entityExists.Id;
        }
        
        public virtual async Task Remove(TId id)
        {
            await using var context = _contextFactory();
            
            var entityExists = await GetById(id);
            context.Set<TEntity>().Remove(entityExists);
            await context.SaveChangesAsync();
        }
    }
}