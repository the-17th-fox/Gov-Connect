using Civilians.Core.Models;
using Civilians.Core.ViewModels.Passports;

namespace Civilians.Application.Interfaces
{
    public interface IPassportsService
    {
        public Task<Passport> GetByIdAsync(Guid id);
        public Task<Passport> GetByPersonalInfo(SearchPassportViewModel passportViewModel);
        public Task UpdateAsync(UpdatePassportViewModel passportViewModel);
    }
}
