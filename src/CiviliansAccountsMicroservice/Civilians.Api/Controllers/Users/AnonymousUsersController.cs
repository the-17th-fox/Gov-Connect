using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Civilians;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Users
{
    [Route("api/public/users")]
    [ApiController]
    public class AnonymousUsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AnonymousUsersController(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationViewModel registrationViewModel)
        {
            await _usersService.CreateAsync(registrationViewModel);
            
            return Created("register", null);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel loginViewModel)
        {
            var tokensPair = await _usersService.LoginAsync(loginViewModel);
            
            return Ok(tokensPair);
        }
    }
}
