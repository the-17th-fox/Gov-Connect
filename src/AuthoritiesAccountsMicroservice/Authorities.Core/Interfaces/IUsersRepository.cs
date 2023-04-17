using Authorities.Core.Models;

namespace Authorities.Core.Interfaces
{
    public interface IUsersRepository
    {
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByEmailAsync(string email);
        public Task<List<User>> GetAllAsync(short pageNumber, byte pageSize, bool showDeleted = false, bool showBlocked = false);
        public Task<List<User>> GetNotConfirmedAsync(short pageNumber, byte pageSize);
    }
}
