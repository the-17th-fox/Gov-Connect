using AutoMapper;
using Civilians.Api.ViewModels.Pagination;
using Civilians.Api.ViewModels.Users;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Civilians;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Users
{
    [Route("api/users/admin")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public AdminUsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _usersService.GetRolesAsync(user);

            return Ok(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllAsync([FromBody] UsersPaginationParametersViewModel paginationParameters)
        {
            var users = await _usersService.GetAllAsync(paginationParameters);
            var usersPage = _mapper.Map<PageViewModel<ShortUserViewModel>>(users);

            return Ok(usersPage);
        }

        [HttpPatch("{id}/block")]
        public async Task<IActionResult> BlockAsync(Guid id)
        {
            await _usersService.BlockAsync(id);

            return Ok();
        }

        [HttpPatch("{id}/unblock")]
        public async Task<IActionResult> UnblockAsync(Guid id)
        {
            await _usersService.UnblockAsync(id);

            return Ok();
        }

        [HttpPatch("{id}/set-role/{roleName}")]
        public async Task<IActionResult> ChangeRoleAsync(Guid id, string newRoleName)
        {
            await _usersService.ChangeRoleAsync(id, newRoleName);

            return Ok();
        }
    }
}
