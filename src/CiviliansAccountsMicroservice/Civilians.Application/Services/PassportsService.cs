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

        public async Task<Passport> GetByUserIdAsync(Guid userId)
        {
            var passport = await  _unitOfWork.PassportsRepository.GetByUserIdAsync(userId);
            if (passport == null)
            {
                throw new KeyNotFoundException("Passport with the specified user id hasn't been found.");
            }

            return passport;
        }

        public async Task<Passport> GetByPersonalDataAsync(SearchPassportViewModel passportViewModel)
        {
            var passport = await _unitOfWork.PassportsRepository.GetByPersonalDataAsync(
                passportViewModel.FirstName, 
                passportViewModel.LastName, 
                passportViewModel.Patronymic);

            if (passport == null)
            {
                throw new KeyNotFoundException("Passport with the specified information hasn't been found.");
            }

            return passport;
        }

        public async Task UpdateAsync(Guid userId, UpdatePassportViewModel passportViewModel)
        {
            var passport = await GetByUserIdAsync(userId);

            if (passportViewModel.RegionCode == RegionCodes.Undefined)
            {
                throw new ArgumentException("Region code is undefined.");
            }

            passport = _mapper.Map<Passport>(passportViewModel);    

            _unitOfWork.PassportsRepository.Update(passport);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
