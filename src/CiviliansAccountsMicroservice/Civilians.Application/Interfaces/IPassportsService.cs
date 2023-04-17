using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Models;

namespace Civilians.Application.Interfaces
{
    public interface IPassportsService
    {
        public Task<Passport> GetByUserIdAsync(Guid userId);
        public Task<Passport> GetByPersonalDataAsync(SearchPassportViewModel passportViewModel);
        public Task UpdateAsync(Guid userId, UpdatePassportViewModel passportViewModel);
    }
}
