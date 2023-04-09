using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface IPassportsRepository
    {
        public Task<Passport?> GetByIdAsync(Guid id);
        public Task<Passport?> GetByPersonalInfoAsync(string firstName, string lastName, string patronymic);
        public Task UpdateAsync(Passport passport);
        public Task CreateAsync(Passport passport);
        public Task SaveChangesAsync();
    }
}
