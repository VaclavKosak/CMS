using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Reporitories;
using CMS.Models.MenuItem;

namespace CMS.BL.Facades
{
    public class MenuItemFacade: FacadeBase<MenuItemListModel, MenuItemDetailModel, MenuItemNewModel, MenuItemUpdateModel, 
        MenuItemRepository, MenuItemEntity, Guid>
    {
        public MenuItemFacade(MenuItemRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
        
        public async Task<List<MenuItemListModel>> GetAll(Guid parentId)
        {
            return Mapper.Map<List<MenuItemListModel>>(await Repository.GetAll(parentId));
        }
        
        public async Task<MenuItemDetailModel> GetDetailDataById(Guid id)
        {
            var entity = await Repository.GetById(id);
            var detailData = Mapper.Map<MenuItemDetailModel>(entity);
            detailData.MenuList = await GetAll(detailData.Id);
            return detailData;
        }

        public override async Task<Guid> Create(MenuItemNewModel newModel)
        {
            var order = 0;
            var items = await Repository.GetAll();
            if (items.Count == 0)
            {
                order = 1;
            }
            else
            {
                order = items.Max(m => m.Order) + 1;
            }

            newModel.Order = order;
            var entity = Mapper.Map<MenuItemEntity>(newModel);
            return await Repository.Insert(entity);
        }

        public async Task<bool> ChangeOrder(Guid firstItem, Guid secondItem)
        {
            var firstEntity = await Repository.GetById(firstItem);
            var secondEntity = await Repository.GetById(secondItem);

            if (firstEntity == null || secondEntity == null)
            {
                return false;
            }

            var firstOrder = firstEntity.Order;
            var secondOrder = secondEntity.Order;

            firstEntity.Order = secondOrder;
            secondEntity.Order = firstOrder;

            await Repository.Update(firstEntity);
            await Repository.Update(secondEntity);

            return true;
        }
    }
}