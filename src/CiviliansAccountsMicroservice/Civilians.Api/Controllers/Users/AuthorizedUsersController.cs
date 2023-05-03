using AutoMapper;
using Civilians.Api.Utilities;
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

        private Guid _userId;

        public AuthorizedUsersController(IUsersService usersService, ITokensService tokensService, IMapper mapper)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            InitializeRequestProperties();

            await _tokensService.RevokeRefreshTokenAsync(_userId);

            return Ok();
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyInfoAsync()
        {
            InitializeRequestProperties();

            var user = await _usersService.GetByIdAsync(_userId);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            userViewModel.Roles = await _usersService.GetRolesAsync(user);

            return Ok(userViewModel);
        }

        private void InitializeRequestProperties()
        {
            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
        }
    }
}
