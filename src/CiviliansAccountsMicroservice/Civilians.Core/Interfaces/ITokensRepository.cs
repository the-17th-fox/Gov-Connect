using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface ITokensRepository
    {
        public Task RevokeRefreshTokenAsync(Guid userId);
        public Task RevokeAllRefreshTokensAsync();
        public Task<RefreshToken> GetByUserIdAsync(Guid id);
        public Task IssueNewRefreshTokenAsync(RefreshToken newRefreshToken, out Guid tokenId);
        public Task UpdateExistingRefreshTokenAsync(RefreshToken newRefreshToken, out Guid tokenId);
    }
}
