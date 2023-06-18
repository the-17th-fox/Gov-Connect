using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.Reports.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Redis.Attributes;

namespace Communications.Api.Controllers.Reports
{
    [Route("api/reports/authorities")]
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

        [Cached(5)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPendingAsync(short pageNumber, byte pageSize)
        {
            var getAllPendingReportsQuery = new GetAllPendingReportsQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var reports = await _mediator.Send(getAllPendingReportsQuery);

            var reportsPage = _mapper.Map<PageViewModel<ShortReportViewModel>>(reports);

            return Ok(reportsPage);
        }
    }
}
