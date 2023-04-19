using Authorities.Core.Interfaces;
using Authorities.Core.Models;
using Authorities.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Infrastructure.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly AuthoritiesDbContext _dbContext;

        public TokensRepository(AuthoritiesDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<RefreshToken?> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);
        }

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
