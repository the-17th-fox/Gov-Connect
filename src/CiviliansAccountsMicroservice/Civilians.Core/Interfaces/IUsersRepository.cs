using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<List<User>> GetAllAsync(short pageNumber, byte pageSize, bool showDeleted = false, bool showBlocked = false);
        public Task UpdateAsync(User user);
    }
}
