using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Civilians.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;

        public UsersRepository(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<List<User>> GetAllAsync(short pageNumber, byte pageSize, bool showDeleted = false, bool showBlocked = false)
        {
            var query = _userManager.Users;

            if (!showDeleted)
            {
                query = query.Where(u => u.IsDeleted == false);
            }

            if (!showBlocked)
            {
                query = query.Where(u => u.LockoutEnabled == false);
            }

            return await PagedList<User>.ToPagedListAsync(query, pageNumber, pageSize);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Include(a => a.RefreshToken)
                .Include(a => a.Passport)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Include(a => a.RefreshToken)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
