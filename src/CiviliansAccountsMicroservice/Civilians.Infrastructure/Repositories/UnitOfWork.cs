using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Civilians.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;

namespace Civilians.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CiviliansDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        private IPassportsRepository _passportsRepository = null!;
        private ITokensRepository _tokensRepository = null!;
        private IUsersRepository _usersRepository = null!;

        public UnitOfWork(
            CiviliansDbContext dbContext, 
            UserManager<User> userManager)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager ));
        }

        public IPassportsRepository PassportsRepository
        {
            get
            {
                _passportsRepository ??= new PassportsRepository(_dbContext);

                return _passportsRepository;
            }
        }

        public ITokensRepository TokensRepository
        {
            get
            {
                _tokensRepository ??= new TokensRepository(_dbContext);

                return _tokensRepository;
            }
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                _usersRepository ??= new UsersRepository(_userManager);

                return _usersRepository!;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
