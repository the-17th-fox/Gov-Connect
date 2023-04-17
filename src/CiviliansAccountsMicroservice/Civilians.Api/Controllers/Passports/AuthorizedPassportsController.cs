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

        private Guid _userId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public AuthorizedPassportsController(IPassportsService passportsService, IMapper mapper)
        {
            _passportsService = passportsService ?? throw new ArgumentNullException(nameof(passportsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyPassportAsync()
        {
            var passport = await _passportsService.GetByUserIdAsync(_userId);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);
            return Ok(passportViewModel);
        }

        [HttpPut("update/my")]
        public async Task<IActionResult> UpdatePassportAsync(UpdatePassportViewModel updatePassportViewModel)
        {
            await _passportsService.UpdateAsync(_userId, updatePassportViewModel);
            return Ok();
        }
    }
}
