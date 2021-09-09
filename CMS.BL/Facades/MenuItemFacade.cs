using System;
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

        public override async Task<Guid> Create(MenuItemNewModel newModel)
        {
            newModel.Order = (await Repository.GetAll()).Max(m => m.Order) + 1;
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