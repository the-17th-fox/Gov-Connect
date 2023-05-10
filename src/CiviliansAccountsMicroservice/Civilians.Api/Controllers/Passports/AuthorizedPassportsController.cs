using AutoMapper;
using Civilians.Api.Utilities;
using Civilians.Api.ViewModels.Passports;
using Civilians.Application.Interfaces;
using Civilians.Application.ViewModels.Passports;
using Microsoft.AspNetCore.Mvc;

namespace Civilians.Api.Controllers.Passports
{
    [Route("api/passports/authorized")]
    [ApiController]
    public class AuthorizedPassportsController : ControllerBase
    {
        private readonly IPassportsService _passportsService;
        private readonly IMapper _mapper;

        private Guid _userId;

        public AuthorizedPassportsController(IPassportsService passportsService, IMapper mapper)
        {
            _passportsService = passportsService ?? throw new ArgumentNullException(nameof(passportsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyPassportAsync()
        {
            InitializeRequestProperties();

            var passport = await _passportsService.GetByUserIdAsync(_userId);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);

            return Ok(passportViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassportAsync(UpdatePassportViewModel updatePassportViewModel)
        {
            InitializeRequestProperties();

            await _passportsService.UpdateAsync(_userId, updatePassportViewModel);

            return Ok();
        }

        private void InitializeRequestProperties()
        {
            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
        }
    }
}
