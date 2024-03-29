﻿using AutoMapper;
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

        public AuthorizedPassportsController(IPassportsService passportsService, IMapper mapper)
        {
            _passportsService = passportsService ?? throw new ArgumentNullException(nameof(passportsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetMyPassportAsync([FromHeader(Name = "uid")] Guid userId)
        {
            var passport = await _passportsService.GetByUserIdAsync(userId);
            var passportViewModel = _mapper.Map<PassportViewModel>(passport);

            return Ok(passportViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassportAsync(
            [FromHeader(Name = "uid")] Guid userId, 
            UpdatePassportViewModel updatePassportViewModel)
        {
            await _passportsService.UpdateAsync(userId, updatePassportViewModel);

            return Ok();
        }
    }
}
