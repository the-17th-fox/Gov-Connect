using AutoMapper;
using Civilians.Api.ViewModels.Passports;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Passports;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Passports
{
    [Route("api/passports/admin")]
    [ApiController]
    public class AdminPassportsController : ControllerBase
    {
        private readonly IPassportsService _passportsService;
        private readonly IMapper _mapper;

        public AdminPassportsController(IPassportsService passportsService, IMapper mapper)
        {
            _passportsService = passportsService ?? throw new ArgumentNullException(nameof(passportsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("{lastName}-{firstName}-{patronymic}")]
        public async Task<IActionResult> GetByPersonalDataAsync(string lastName, string firstName, string patronymic)
        {
            var searchPassportViewModel = new SearchPassportViewModel()
            {
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic
            };

            var passport = await _passportsService.GetByPersonalDataAsync(searchPassportViewModel);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);
            
            return Ok(passportViewModel);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(Guid userId)
        {
            var passport = await _passportsService.GetByUserIdAsync(userId);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);
            
            return Ok(passportViewModel);
        }
    }
}
