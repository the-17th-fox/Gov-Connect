using AutoMapper;
using Civilians.Api.ViewModels.Users;
using Civilians.Application.Interfaces;
using Civilians.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Civilians.Api.Controllers.Users
{
    [Authorize(Policy = AuthPolicies.DefaultRights)]
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

        [HttpGet("profile")]
        public async Task<IActionResult> GetMyInfoAsync()
        {
            var user = await _usersService.GetByIdAsync(_userId);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _usersService.GetRolesAsync(user);

            return Ok(userViewModel);
        }
    }
}
