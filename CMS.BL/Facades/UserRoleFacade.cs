using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CMS.DAL.Entities;
using CMS.DAL.Repositories;
using CMS.Models.UserRole;

namespace CMS.BL.Facades
{
    public class UserRoleFacade : FacadeBase<UserRoleModel, UserRoleModel, UserRoleModel, UserRoleModel, 
        UserRoleRepository, AppUserRole, Guid>
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        public UserRoleFacade(UserRoleRepository repository, IMapper mapper, UserRepository userRepository, RoleRepository roleRepository) : base(repository, mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public override async Task<IList<UserRoleModel>> GetAll()
        {
            var items = await Repository.GetAll();
            foreach (var item in items)
            {
                item.Role = await _roleRepository.GetById(item.RoleId);
                item.User = await _userRepository.GetById(item.UserId);
            }
            return Mapper.Map<IList<UserRoleModel>>(items);
        }
    }
}