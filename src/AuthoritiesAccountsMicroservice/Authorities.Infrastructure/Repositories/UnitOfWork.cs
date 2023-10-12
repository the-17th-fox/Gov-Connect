using Authorities.Core.Interfaces;
using Authorities.Core.Models;
using Authorities.Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;

namespace Authorities.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthoritiesDbContext _dbContext;

        private ITokensRepository _tokensRepository = null!;
        
        private IUsersRepository _usersRepository = null!;
        private readonly UserManager<User> _userManager;

        public UnitOfWork(AuthoritiesDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
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
				
                return _usersRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
