using Authorities.Api.ViewModels.Pagination;
using Authorities.Api.ViewModels.Users;
using Authorities.Application.Interfaces;
using Authorities.Application.ViewModels.Pagination;
using Authorities.Application.ViewModels.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Authorities.Api.Controllers.Users
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
        public async Task<IActionResult> GetAllAsync(UsersPaginationParametersViewModel paginationParameters)
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

        [HttpPatch("{id}/roles/set-role/{newRoleName}")]
        public async Task<IActionResult> ChangeRoleAsync(Guid id, string newRoleName)
        {
            await _usersService.ChangeRolesAsync(id, newRoleName);

            return Ok();
        }

        [HttpPatch("{id}/roles/remove")]
        public async Task<IActionResult> RemoveRolesAsync(Guid id)
        {
            await _usersService.RemoveRolesAsync(id);

            return Ok();
        }

        [HttpPost("not-confirmed")]
        public async Task<IActionResult> GetNotConfirmedAsync(PaginationParametersViewModel paginationParameters)
        {
            var users = await _usersService.GetNotConfirmedAsync(paginationParameters);
            var usersPage = _mapper.Map<PageViewModel<ShortUserViewModel>>(users);

            return Ok(usersPage);
        }
    }
}
