using AutoMapper;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Interfaces;
using Civilians.Core.Misc;
using Civilians.Core.Models;
using SharedLib.ExceptionsHandler.CustomExceptions;
using SharedLib.Kafka.Interfaces;

namespace Civilians.Application.Services
{
    public class PassportsService : IPassportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer<string, object> _producer;

        public PassportsService(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducer<string, object> producer)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public async Task<Passport> GetByUserIdAsync(Guid userId)
        {
            var passport = await  _unitOfWork.PassportsRepository.GetByUserIdAsync(userId);
            if (passport == null)
            {
                throw new NotFoundException("Passport with the specified user id hasn't been found.");
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
                throw new BadRequestException("Region code is undefined.");
            }

            _mapper.Map(passportViewModel, passport);

            _unitOfWork.PassportsRepository.Update(passport);

            await _unitOfWork.SaveChangesAsync();

            await SendUpdateNotification(passport.UserId, passport.FirstName, passport.Patronymic);
        }

        private async Task SendUpdateNotification(Guid civilianId, string firstName, string patronymic)
        {
            var message = new
            {
                CivilianId = civilianId,
                FirstName = firstName,
                Patronymic = patronymic
            };

            await _producer.ProduceAsync(civilianId.ToString(), message);
        }
    }
}
