using Civilians.Application.Interfaces;
using Civilians.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;
using Civilians.Application.ViewModels.Passports;
using Civilians.Api.ViewModels.Passports;

namespace Civilians.Api.Controllers.Passports
{
    [Authorize(Policy = AuthPolicies.DefaultRights)]
    [Route("api/passports")]
    [ApiController]
    public class AuthorizedPassportsController : ControllerBase
    {
        private readonly IPassportsService _passportsService;
        private readonly IMapper _mapper;

        private readonly Guid _userId;

        public AuthorizedPassportsController(IPassportsService passportsService, IMapper mapper)
        {
            _passportsService = passportsService ?? throw new ArgumentNullException(nameof(passportsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            var userIdAsString = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                 throw new ArgumentNullException(nameof(ClaimTypes.NameIdentifier));
            _userId = Guid.Parse(userIdAsString);
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyPassportAsync()
        {
            var passport = await _passportsService.GetByUserIdAsync(_userId);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);

            return Ok(passportViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassportAsync(UpdatePassportViewModel updatePassportViewModel)
        {
            await _passportsService.UpdateAsync(_userId, updatePassportViewModel);

            return Ok();
        }
    }
}
