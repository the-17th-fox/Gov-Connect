using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Reports
{
    [Route("api/reports")]
    [ApiController]
    public class AuthoritiesReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthoritiesReportsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("pending")]
        public async Task<IActionResult> GetAllPendingAsync(GetAllPendingReportsQuery getAllPendingReportsQuery)
        {
            var reports = await _mediator.Send(getAllPendingReportsQuery);

            var reportsPage = _mapper.Map<PageViewModel<ShortReportViewModel>>(reports);

            return Ok(reportsPage);
        }
    }
}
