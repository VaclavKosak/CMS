using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.MenuItem;

namespace CMS.BL.Facades;

public class MenuItemFacade(MenuItemRepository repository, IMapper mapper)
    : FacadeBase<MenuItemModel, MenuItemModel, MenuItemModel, MenuItemModel,
        MenuItemRepository, MenuItemEntity, Guid>(repository, mapper)
{
    public async Task<List<MenuItemModel>> GetAll(Guid parentId)
    {
        return Mapper.Map<List<MenuItemModel>>(await Repository.GetAll(parentId));
    }

    public async Task<MenuItemModel> GetDetailDataById(Guid id)
    {
        var entity = await Repository.GetById(id);
        var detailData = Mapper.Map<MenuItemModel>(entity);
        detailData.MenuList = await GetAll(detailData.Id);
        return detailData;
    }

    public override async Task<Guid> Create(MenuItemModel newModel)
    {
        int order;
        var items = await Repository.GetAll();
        if (items.Count == 0)
            order = 1;
        else
            order = items.Max(m => m.Order) + 1;

        newModel.Order = order;
        var entity = Mapper.Map<MenuItemEntity>(newModel);
        return await Repository.Insert(entity);
    }

    public async Task ChangeOrder(Guid firstItem, Guid secondItem)
    {
        var firstEntity = await Repository.GetById(firstItem);
        var secondEntity = await Repository.GetById(secondItem);

        if (firstEntity == null || secondEntity == null) return;

        var firstOrder = firstEntity.Order;
        var secondOrder = secondEntity.Order;

        firstEntity.Order = secondOrder;
        secondEntity.Order = firstOrder;

        await Repository.Update(firstEntity);
        await Repository.Update(secondEntity);
    }
}