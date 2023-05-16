using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.Reports.Queries;
using Communications.Application.Reports.Queries.GetReportById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Reports
{
    [Route("api/reports/public")]
    [ApiController]
    public class AnonymousReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AnonymousReportsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var report = await _mediator.Send(new GetReportByIdQuery() { Id = id });

            var reportViewModel = _mapper.Map<PublicReportViewModel>(report);

            return Ok(reportViewModel);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetAllAsync(GetAllReportsQuery getAllReportsQuery)
        {
            var reports = await _mediator.Send(getAllReportsQuery);

            var reportsPage = _mapper.Map<PageViewModel<ShortReportViewModel>>(reports);

            return Ok(reportsPage);
        }
    }
}
