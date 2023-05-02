using AutoMapper;
using Communications.Api.Utilities;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.BaseMethods;
using Communications.Application.Reports.Commands;
using Communications.Application.Reports.Queries;
using Communications.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Reports
{
    [Route("api/reports")]
    [ApiController]
    public class CiviliansReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private Guid _userId;
        private string _firstName = string.Empty;
        private string _patronymic = string.Empty;

        public CiviliansReportsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync(CreateReportViewModel createReportViewModel)
        {
            InitializeRequestProperties();

            var createReportCommand = _mapper.Map<CreateReportCommand>(createReportViewModel);
            createReportCommand.CivilianId = _userId;
            createReportCommand.FirstName = _firstName;
            createReportCommand.Patronymic = _patronymic;

            await _mediator.Send(createReportCommand);

            return Created("new", null);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateReportViewModel updateReportViewModel)
        {
            var updateReportCommand = _mapper.Map<UpdateReportCommand>(updateReportViewModel);
            updateReportCommand.Id = id;

            await _mediator.Send(updateReportCommand);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteReportCommand() { Id = id });

            return Ok();
        }

        [HttpPost("personal")]
        public async Task<IActionResult> GetAllByCivilianAsync(BaseGetAllQuery<Report> baseGetAllQuery)
        {
            InitializeRequestProperties();

            var getAllReportsByCivilianQuery = _mapper.Map<GetAllReportsByCivilianQuery>(baseGetAllQuery);
            getAllReportsByCivilianQuery.CivilianId = _userId;

            var reports = await _mediator.Send(getAllReportsByCivilianQuery);

            var reportsPage = _mapper.Map<PageViewModel<ShortReportViewModel>>(reports);

            return Ok(reportsPage);
        }

        private void InitializeRequestProperties()
        {
            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
            _firstName = HttpContext.GetValueFromHeader("fname");
            _patronymic = HttpContext.GetValueFromHeader("pname");
        }
    }
}
