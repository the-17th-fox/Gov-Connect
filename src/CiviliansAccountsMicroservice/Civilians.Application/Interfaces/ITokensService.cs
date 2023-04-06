using Civilians.Core.Models;
using Civilians.Core.ViewModels.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Civilians.Application.Interfaces
{
    public interface ITokensService
    {
        // Access tokens management
        public JwtSecurityToken CreateAccessToken(IList<Claim> claims);
        public Task<TokensViewModel> RefreshAccessTokenAsync(TokensRefreshingViewModel tokensPairViewModel);

        // Refresh tokens management
        public Task<RefreshToken> IssueRefreshTokenAsync(Guid userId);
        public Task RevokeRefreshTokenAsync(Guid userId);
        public Task RevokeAllRefreshTokensAsync();
    }
}
