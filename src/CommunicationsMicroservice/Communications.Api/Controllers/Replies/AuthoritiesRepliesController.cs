using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Replies;
using Communications.Application.Replies.Commands;
using Communications.Application.Replies.Queries;
using Communications.SignalR.Extensions;
using Communications.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SharedLib.Redis.Attributes;

namespace Communications.Api.Controllers.Replies
{
    [Route("api/replies/authorities")]
    [ApiController]
    public class AuthoritiesRepliesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private readonly IHubContext<RepliesHub> _repliesHubContext;

        public AuthoritiesRepliesController(IMediator mediator, IMapper mapper, IHubContext<RepliesHub> repliesHubContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repliesHubContext = repliesHubContext ?? throw new ArgumentNullException(nameof(repliesHubContext));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            [FromHeader(Name = "role")] string organization,
            [FromHeader(Name = "uid")] Guid userId, 
            CreateReplyViewModel createReplyViewModel)
        {
            var createReplyCommand = _mapper.Map<CreateReplyCommand>(createReplyViewModel);
            createReplyCommand.AuthorityId = userId;
            createReplyCommand.Organization = organization;

            await _mediator.Send(createReplyCommand);

            await SendReplyToGroupAsync(createReplyCommand);

            return Created("new", null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteReplyCommand() { Id = id });

            return Ok();
        }

        [Cached]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync(short pageNumber, byte pageSize)
        {
            var getAllRepliesQuery = new GetAllRepliesQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var replies = await _mediator.Send(getAllRepliesQuery);

            var repliesPage = _mapper.Map<PageViewModel<ShortReplyViewModel>>(replies); 

            return Ok(repliesPage);
        }

        [Cached(10)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var reply = await _mediator.Send(new GetReplyByIdQuery() { Id = id });

            var replyViewModel = _mapper.Map<PublicReplyViewModel>(reply);

            return Ok(replyViewModel);
        }

        private async Task SendReplyToGroupAsync(CreateReplyCommand reply)
        {
            await _repliesHubContext.SendUpdateToGroup(
                groupName: reply.ReportId.ToString(),
                method: BaseCommunicationsHub.NewMessageMethodName,
                message: reply);
        }
    }
}
