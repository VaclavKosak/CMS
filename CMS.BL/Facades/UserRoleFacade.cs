using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.UserRole;

namespace CMS.BL.Facades;

public class UserRoleFacade(
    UserRoleRepository repository,
    IMapper mapper,
    UserRepository userRepository,
    RoleRepository roleRepository)
    : FacadeBase<UserRoleModel, UserRoleModel, UserRoleModel, UserRoleModel,
        UserRoleRepository, AppUserRole, Guid>(repository, mapper)
{
    public override async Task<IList<UserRoleModel>> GetAll()
    {
        var items = await Repository.GetAll();
        foreach (var item in items)
        {
            item.Role = await roleRepository.GetById(item.RoleId);
            item.User = await userRepository.GetById(item.UserId);
        }

        return Mapper.Map<IList<UserRoleModel>>(items);
    }
}