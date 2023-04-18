using Authorities.Application.Interfaces;
using Authorities.Application.ViewModels.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorities.Api.Controllers.Tokens
{
    [AllowAnonymous]
    [Route("api/tokens")]
    [ApiController]
    public class AnonymousTokensController : ControllerBase
    {
        private readonly ITokensService _tokensService;

        public AnonymousTokensController(ITokensService tokensService)
        {
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessTokenAsync([FromBody] TokensRefreshingViewModel tokensRefreshingViewModel)
        {
            var tokensPair = await _tokensService.RefreshAccessTokenAsync(tokensRefreshingViewModel);
         
            return Ok(tokensPair);
        }
    }
}
