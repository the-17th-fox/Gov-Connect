﻿using Authorities.Core.Interfaces;
using Authorities.Core.Models;
using Authorities.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorities.Infrastructure.Repositories
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
            var query = _userManager.Users
                .AsNoTracking();

            if (!showDeleted)
            {
                query = query.Where(u => u.IsDeleted == false);
            }

            if (!showBlocked)
            {
                query = query.Where(u => u.LockoutEnabled == false);
            }

            return await PagedList<User>.ToPagedListAsync(query, pageNumber, pageSize, u => u.Id);
        }

        public async Task<List<User>> GetNotConfirmedAsync(short pageNumber, byte pageSize)
        {
            var query = _userManager.Users
                .AsNoTracking()
                .Where(u => u.IsBlocked == false)
                .Where(u => u.IsDeleted == false)
                .Where(u => u.IsConfirmed == false);

            return await PagedList<User>.ToPagedListAsync(query, pageNumber, pageSize, u => u.Id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Include(a => a.RefreshToken)
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userManager.Users
                .Include(a => a.RefreshToken)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
