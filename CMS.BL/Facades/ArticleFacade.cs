using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Article;

namespace CMS.BL.Facades;

public class ArticleFacade(ArticleRepository repository, IMapper mapper, CategoryRepository categoryRepository)
    : FacadeBase<ArticleListModel, ArticleDetailModel, ArticleNewModel, ArticleUpdateModel,
        ArticleRepository, ArticleEntity, Guid>(repository, mapper)
{
    public override async Task<Guid> Create(ArticleNewModel newModel)
    {
        // insert article
        var entity = Mapper.Map<ArticleEntity>(newModel);
        var itemId = await Repository.Insert(entity);
            
        // insert category
        newModel.CategoriesList ??= new List<Guid>();
        var categories = await categoryRepository.GetAllByIds(newModel.CategoriesList.ToArray());
        await Repository.Update(entity, categories);
            
        return itemId;
    }
        
    public override async Task<Guid> Update(ArticleUpdateModel updateModel)
    {
        var entity = Mapper.Map<ArticleEntity>(updateModel);
        await Repository.Update(entity);
            
        var originalEntity = await Repository.GetById(updateModel.Id);
        entity.Category = new List<CategoryEntity>();
        foreach (var category in originalEntity.Category)
        {
            entity.Category.Add(category);
        }
            
        updateModel.CategoriesList ??= new List<Guid>();
        var categories = await categoryRepository.GetAllByIds(updateModel.CategoriesList.ToArray());

        return await Repository.Update(entity, categories);
    }
        
    public async Task<ArticleDetailModel> GetByUrl(string url)
    {
        var entity = await Repository.GetByUrl(url);
        return Mapper.Map<ArticleDetailModel>(entity);
    }
}