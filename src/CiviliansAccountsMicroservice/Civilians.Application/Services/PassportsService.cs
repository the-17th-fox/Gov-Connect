using AutoMapper;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Interfaces;
using Civilians.Core.Misc;
using Civilians.Core.Models;

namespace Civilians.Application.Services
{
    public class PassportsService : IPassportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PassportsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Passport> GetByIdAsync(Guid id)
        {
            var passport = await GetIfExistsAsync(id);

            return passport;
        }

        public async Task<Passport> GetByPersonalInfo(SearchPassportViewModel passportViewModel)
        {
            var passport = await _unitOfWork.PassportsRepository.GetByPersonalInfoAsync(
                passportViewModel.FirstName, 
                passportViewModel.LastName, 
                passportViewModel.Patronymic);

            if (passport == null)
            {
                throw new KeyNotFoundException("Passport with the specified information hasn't been found.");
            }

            return passport;
        }

        public async Task UpdateAsync(UpdatePassportViewModel passportViewModel)
        {
            var passport = await GetIfExistsAsync(passportViewModel.Id);

            if (passportViewModel.RegionCode == RegionCodes.Undefined)
            {
                throw new ArgumentException("Region code is undefined.");
            }

            passport = _mapper.Map<Passport>(passportViewModel);    

            _unitOfWork.PassportsRepository.Update(passport);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Passport> GetIfExistsAsync(Guid id)
        {
            var passport = await _unitOfWork.PassportsRepository.GetByIdAsync(id);
            if (passport == null)
            {
                throw new KeyNotFoundException("Passport with the specified id hasn't been found.");
            }    

            return passport;
        }
    }
}
