using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Civilians.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CiviliansDbContext _context;
        private readonly UserManager<User> _userManager;

        private IPassportsRepository _passportsRepository = null!;
        private ITokensRepository _tokensRepository = null!;
        private IUsersRepository _usersRepository = null!;

        public UnitOfWork(
            CiviliansDbContext context, 
            UserManager<User> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager ));
        }

        public IPassportsRepository PassportsRepository
        {
            get
            {
                if (_passportsRepository is null)
                    _passportsRepository = new PassportsRepository(_context);

                return _passportsRepository;
            }
        }

        public ITokensRepository TokensRepository
        {
            get
            {
                if (_tokensRepository is null)
                    _tokensRepository = new TokensRepository(_context);

                return _tokensRepository;
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                if (_usersRepository is null)
                    _usersRepository = new UsersRepository(_userManager);

                return _usersRepository!;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
