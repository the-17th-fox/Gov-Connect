using Authorities.Application.Interfaces;
using Authorities.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorities.Api.Controllers.Tokens
{
    [Authorize(Policy = AuthPolicies.Administrators)]
    [Route("api/tokens")]
    [ApiController]
    public class AdminTokensController : ControllerBase
    {
        private readonly ITokensService _tokensService;

        public AdminTokensController(ITokensService tokensService)
        {
            _tokensService = tokensService ?? throw new ArgumentNullException(nameof(tokensService));
        }

        [HttpPatch("revoke/{userId}")]
        public async Task<IActionResult> RevokeRefreshTokenAsync(Guid userId)
        {
            await _tokensService.RevokeRefreshTokenAsync(userId);
            
            return Ok();
        }

        [HttpPatch("revoke")]
        public async Task<IActionResult> RevokeAllRefreshTokensAsync()
        {
            await _tokensService.RevokeAllRefreshTokensAsync();
            
            return Ok();
        }
    }
}
