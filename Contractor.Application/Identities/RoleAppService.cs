using AutoMapper;
using Contractor.Exceptions;
using Contractor.Tools;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractor.Identities
{
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IRoleManager _roleManager;
        private readonly IPageRepository _pageRepository;

        public RoleAppService(IRoleRepository roleRepository, IMapper mapper, IRoleManager roleManager, IPageRepository pageRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _pageRepository = pageRepository;
        }

        public async Task<RoleDto> CreateAsync(RoleDto roleDto)
        {
            var role = Role.Create(roleDto.Name, roleDto.IsActive);

            var response = await _roleManager.CreateRoleAsync(role);

            return _mapper.Map<RoleDto>(response);
        }

        public async Task<RoleDto> UpdateAsync(RoleDto roleDto)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleDto.Id);

            role.Update(roleDto.Name, roleDto.IsActive);

            var response = await _roleManager.UpdateRoleAsync(role);

            return _mapper.Map<RoleDto>(response);
        }
        
        /// <summary>
        /// GetAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleDto> GetAsync(string id)
        {
            #region Call Repository
            var role = await _roleRepository.GetRoleByIdAsync(id);
            #endregion

            return _mapper.Map<RoleDto>(role);
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="roleSearchDto"></param>
        /// <returns></returns>
        public async Task<ListServiceModel<RoleDto>> GetListAsync(string? name,
            bool? isActive,
            int pageNumber, int pageSize)
        {
            #region Call Repository
            var roles = await _roleRepository.GetListAsync(name,
                isActive,
                pageNumber, pageSize);
            #endregion

            var rolesDto = _mapper.Map<List<RoleDto>>(roles.List);
            return new ListServiceModel<RoleDto>(roles.TotalCount, rolesDto);
        }

        //TODO need update
        public async Task<RoleDto> UpdateRolePagesAsync(string roleId, List<int> pageIds)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            var pages = await _pageRepository.GetListAsync(pageIds);

            role.UpdatePageClaims(pages.Select(x => new Tuple<int, string>(x.Id, x.Name)).ToList());

            await _roleRepository.SaveChangesAsync();

            return _mapper.Map<RoleDto>(role);
        }
    }
}
