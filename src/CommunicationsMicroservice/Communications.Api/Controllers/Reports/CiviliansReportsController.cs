using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.Reports.Commands;
using Communications.Application.Reports.Queries;
using Communications.SignalR.Extensions;
using Communications.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Communications.Api.Controllers.Reports
{
    [Route("api/reports/civilians")]
    [ApiController]
    public class CiviliansReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private readonly IHubContext<ReportsHub> _reportsHubContext;

        public CiviliansReportsController(
            IMediator mediator, 
            IMapper mapper, 
            IHubContext<ReportsHub> reportsHubContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportsHubContext = reportsHubContext ?? throw new ArgumentNullException(nameof(reportsHubContext));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromHeader(Name = "fname")] string firstName,
            [FromHeader(Name = "pname")] string patronymic,
            [FromHeader(Name = "uid")] Guid userId,
            CreateReportViewModel createReportViewModel)
        {
            var createReportCommand = _mapper.Map<CreateReportCommand>(createReportViewModel);
            createReportCommand.CivilianId = userId;
            createReportCommand.FirstName = firstName;
            createReportCommand.Patronymic = patronymic;

            await _mediator.Send(createReportCommand);

            await SendReportToGroupAsync(createReportCommand);

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

        [HttpGet("personal")]
        public async Task<IActionResult> GetAllByCivilianAsync(
            [FromHeader(Name = "uid")] Guid userId, 
            short pageNumber, 
            byte pageSize)
        {
            var getAllReportsByCivilianQuery = new GetAllReportsByCivilianQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                CivilianId = userId
            };

            var reports = await _mediator.Send(getAllReportsByCivilianQuery);

            var reportsPage = _mapper.Map<PageViewModel<ShortReportViewModel>>(reports);

            return Ok(reportsPage);
        }

        private async Task SendReportToGroupAsync(CreateReportCommand report)
        {
            await _reportsHubContext.SendUpdateToGroup(
                groupName: ReportsHub.GroupName,
                method: BaseCommunicationsHub.NewMessageMethodName,
                message: report);
        }
    }
}
