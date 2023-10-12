using AutoMapper;
using Communications.Api.ViewModels.Replies;
using Communications.Application.Replies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Redis.Attributes;

namespace Communications.Api.Controllers.Replies
{
    [Route("api/replies/public")]
    [ApiController]
    public class AnonymousRepliesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AnonymousRepliesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Cached(5)]
        [HttpGet("reports/{reportId}")]
        public async Task<IActionResult> GetByReportIdAsync(Guid reportId)
        {
            var reply = await _mediator.Send(new GetReplyByReportQuery() { ReportId = reportId });

            var replyViewModel = _mapper.Map<PublicReplyViewModel>(reply);

            return Ok(replyViewModel);
        }
    }
}
