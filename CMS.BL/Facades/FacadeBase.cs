using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CMS.BL.Facades.Interfaces;
using CMS.DAL.Entities.Interfaces;
using CMS.DAL.Repositories;

namespace CMS.BL.Facades;

public class FacadeBase<TListModel, TDetailModel, TNewModel, TUpdateModel, TRepository, TEntity, TId>(
    TRepository repository,
    IMapper mapper) : IAppFacade
    where TRepository : RepositoryBase<TEntity, TId>
    where TEntity : class, IEntity<TId>
{
    protected readonly TRepository Repository = repository;
    protected readonly IMapper Mapper = mapper;

    public virtual async Task<IList<TListModel>> GetAll()
    {
        return Mapper.Map<IList<TListModel>>(await Repository.GetAll());
    }

    public virtual async Task<TDetailModel> GetById(TId id)
    {
        var entity = await Repository.GetById(id);
        return Mapper.Map<TDetailModel>(entity);
    }
        
    public virtual async Task<TUpdateModel> GetEditedById(TId id)
    {
        var entity = await Repository.GetById(id);
        return Mapper.Map<TUpdateModel>(entity);
    }

    public virtual async Task<TId> Create(TNewModel newModel)
    {
        var entity = Mapper.Map<TEntity>(newModel);
        return await Repository.Insert(entity);
    }

    public virtual async Task<TId> Update(TUpdateModel updateModel)
    {
        var entity = Mapper.Map<TEntity>(updateModel);
        return await Repository.Update(entity);
    }

    public virtual async Task Remove(TId id)
    {
        await Repository.Remove(id);
    }
}