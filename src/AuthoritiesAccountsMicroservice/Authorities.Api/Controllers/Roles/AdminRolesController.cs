using Authorities.Api.ViewModels.Pagination;
using Authorities.Api.ViewModels.Roles;
using Authorities.Application.Interfaces;
using Authorities.Application.ViewModels.Pagination;
using Authorities.Core.Auth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Authorities.Api.Controllers.Roles
{
    [Authorize(Policy = AuthPolicies.Administrators)]
    [Route("api/roles")]
    [ApiController]
    public class AdminRolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;

        public AdminRolesController(IRolesService rolesService, IMapper mapper)
        {
            _rolesService = rolesService ?? throw new ArgumentNullException(nameof(rolesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var role = await _rolesService.GetByIdAsync(id);
            var roleViewModel = _mapper.Map<RoleViewModel>(role);

            return Ok(roleViewModel);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var role = await _rolesService.GetByNameAsync(name);
            var roleViewModel = _mapper.Map<RoleViewModel>(role);

            return Ok(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllAsync(PaginationParametersViewModel paginationParameters)
        {
            var roles = await _rolesService.GetAllAsync(paginationParameters);
            var rolesViewModel = _mapper.Map<PageViewModel<RoleViewModel>>(roles);

            return Ok(rolesViewModel);
        }

        [HttpPost("new/{roleName}")]
        public async Task<IActionResult> CreateAsync(string roleName)
        {
            await _rolesService.CreateAsync(roleName);

            return Ok();
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteAsync(string roleName)
        {
            await _rolesService.DeleteAsync(roleName);

            return Ok();
        }

        [HttpPatch("{oldRoleName}")]
        public async Task<IActionResult> UpdateAsync(string oldRoleName, string newRoleName)
        {
            await _rolesService.UpdateAsync(oldRoleName, newRoleName);

            return Ok();
        }
    }
}
