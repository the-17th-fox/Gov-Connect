using AutoMapper;
using Authorities.Api.ViewModels.Users;
using Authorities.Application.Interfaces;
using Authorities.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authorities.Api.Controllers.Users
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class AuthorizedUsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokensService _tokensService;
        private readonly IMapper _mapper;

        private Guid _userId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public AuthorizedUsersController(IUsersService usersService, ITokensService tokensService, IMapper mapper)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _tokensService.RevokeRefreshTokenAsync(_userId);

            return Ok();
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyInfoAsync()
        {
            var user = await _usersService.GetByIdAsync(_userId);

            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _usersService.GetRolesAsync(user);

            return Ok(userViewModel);
        }
    }
}
