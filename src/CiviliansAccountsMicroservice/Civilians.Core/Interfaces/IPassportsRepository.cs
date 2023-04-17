using Civilians.Core.Models;

namespace Civilians.Core.Interfaces
{
    public interface IPassportsRepository
    {
        public Task<Passport?> GetByIdAsync(Guid id);
        public Task<Passport?> GetByUserIdAsync(Guid userId);
        public Task<Passport?> GetByPersonalDataAsync(string firstName, string lastName, string patronymic);
        public void Update(Passport passport);
        public void Create(Passport passport);
    }
}
