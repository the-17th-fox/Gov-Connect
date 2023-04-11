using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface ITokensRepository
    {
        public void RevokeRefreshToken(RefreshToken refreshToken);
        public Task RevokeAllRefreshTokensAsync();
        public Task<RefreshToken?> GetByUserIdAsync(Guid id);
        public void IssueNewRefreshToken(RefreshToken newRefreshToken);
        public void UpdateExistingRefreshToken(RefreshToken newRefreshToken);
    }
}
