using Civilians.Core.Models;
using Civilians.Core.ViewModels.Civilians;

namespace Civilians.Core.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<List<User>> GetAllAsync(UsersPaginationParametersViewModel pageParams);
        public Task BlockAsync(User user);
        public Task UnblockAsync(User user);
    }
}
