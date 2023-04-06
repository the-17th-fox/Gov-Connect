using Civilians.Application.Interfaces;
using Civilians.Core.Interfaces;
using Civilians.Core.Misc;
using Civilians.Core.Models;
using Civilians.Core.ViewModels.Passports;

namespace Civilians.Application.Services
{
    public class PassportsService : IPassportsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PassportsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Passport> GetByIdAsync(Guid id)
        {
            var passport = await GetIfExistsAsync(id);

            return passport;
        }

        public async Task<Passport> GetByPersonalInfo(SearchPassportViewModel passportViewModel)
        {
            var passport = await _unitOfWork.PassportsRepository.GetByPersonalInfoAsync(passportViewModel);
            if (passport == null)
                throw new KeyNotFoundException("Passport with the specified information hasn't been found.");

            return passport;
        }

        public async Task UpdateAsync(UpdatePassportViewModel passportViewModel)
        {
            var passport = await GetIfExistsAsync(passportViewModel.Id);

            if (passportViewModel.RegionCode == RegionCodes.Undefined)
                throw new ArgumentException("Region code is undefined.");

            await _unitOfWork.PassportsRepository.UpdateAsync(passportViewModel);
        }

        private async Task<Passport> GetIfExistsAsync(Guid id)
        {
            var passport = await _unitOfWork.PassportsRepository.GetByIdAsync(id);
            if (passport == null)
                throw new KeyNotFoundException("Passport with the specified id hasn't been found.");

            return passport;
        }
    }
}
