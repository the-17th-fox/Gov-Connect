using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface ITokensRepository
    {
        public Task RevokeRefreshTokenAsync(Guid userId);
        public Task RevokeAllRefreshTokensAsync();
        public Task<RefreshToken> IssueNewRefreshTokenAsync(RefreshToken newRefreshToken);
    }
}
