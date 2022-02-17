using System;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.Category;

namespace CMS.BL.Facades
{
    public class CategoryFacade : FacadeBase<CategoryListModel, CategoryDetailModel, CategoryNewModel, CategoryUpdateModel, 
        CategoryRepository, CategoryEntity, Guid>
    {
        public CategoryFacade(CategoryRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}