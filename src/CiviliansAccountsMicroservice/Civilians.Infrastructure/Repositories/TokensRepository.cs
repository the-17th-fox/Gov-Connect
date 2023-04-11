using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly CiviliansDbContext _context;

        public TokensRepository(CiviliansDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<RefreshToken?> GetByUserIdAsync(Guid id)
            => await _context.RefreshTokens.FindAsync(id);

        public void IssueNewRefreshToken(RefreshToken newRefreshToken)
            => _context.RefreshTokens.Add(newRefreshToken);

        public void UpdateExistingRefreshToken(RefreshToken newRefreshToken)
            => _context.RefreshTokens.Update(newRefreshToken);

        public async Task RevokeAllRefreshTokensAsync()
            => await _context.RefreshTokens.ForEachAsync(rt => rt.IsRevoked = true);

        public void RevokeRefreshToken(RefreshToken refreshToken)
            => _context.RefreshTokens.Update(refreshToken);

    }
}
