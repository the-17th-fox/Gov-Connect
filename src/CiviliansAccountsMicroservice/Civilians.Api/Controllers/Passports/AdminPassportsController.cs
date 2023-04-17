using AutoMapper;
using Civilians.Api.ViewModels.Passports;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Passports
{
    [Authorize(Policy = AuthPolicies.Administrators)]
    [Route("api/passports")]
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

        [HttpGet("{personal-data}")]
        public async Task<IActionResult> GetByPersonalDataAsync(SearchPassportViewModel searchPassportViewModel)
        {
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
