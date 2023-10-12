using AutoMapper;
using Civilians.Api.ViewModels.Users;
using Civilians.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Users
{
    [Route("api/authorized/users")]
    [ApiController]
    public class AuthorizedUsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;

        public AuthorizedUsersController(IUsersService usersService, ITokensService tokensService, IMapper mapper)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromHeader(Name = "uid")] Guid userId)
        {
            await _tokensService.RevokeRefreshTokenAsync(userId);

            return Ok();
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyInfoAsync([FromHeader(Name = "uid")] Guid userId)
        {
            var user = await _usersService.GetByIdAsync(userId);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _usersService.GetRolesAsync(user);

            return Ok(userViewModel);
        }
    }
}
