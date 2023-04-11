using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Civilians.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly CiviliansDbContext _dbContext;

        public TokensRepository(CiviliansDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<RefreshToken?> GetByUserIdAsync(Guid id)
            => await _dbContext.RefreshTokens.FindAsync(id);

        public void IssueNewRefreshToken(RefreshToken newRefreshToken)
            => _dbContext.RefreshTokens.Add(newRefreshToken);

        public void UpdateExistingRefreshToken(RefreshToken newRefreshToken)
            => _dbContext.RefreshTokens.Update(newRefreshToken);

        public async Task RevokeAllRefreshTokensAsync()
            => await _dbContext.RefreshTokens.ForEachAsync(rt => rt.IsRevoked = true);

        public void RevokeRefreshToken(RefreshToken refreshToken)
            => _dbContext.RefreshTokens.Update(refreshToken);

    }
}
