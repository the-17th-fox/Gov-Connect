using AutoMapper;
using Communications.Api.Utilities;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Replies;
using Communications.Application.Replies.Commands;
using Communications.Application.Replies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Replies
{
    [Route("api/replies")]
    [ApiController]
    public class AuthoritiesRepliesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private string _organization = string.Empty;
        private Guid _userId;

        public AuthoritiesRepliesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync(CreateReplyViewModel createReplyViewModel)
        {
            var createReplyCommand = _mapper.Map<CreateReplyCommand>(createReplyViewModel);
            createReplyCommand.AuthorityId = _userId;
            createReplyCommand.Organization = _organization;

            await _mediator.Send(createReplyCommand);

            return Created("new", null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteReplyCommand() { Id = id });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllAsync(GetAllRepliesQuery getAllRepliesQuery)
        {
            var replies = await _mediator.Send(getAllRepliesQuery);

            var repliesPage = _mapper.Map<PageViewModel<ShortReplyViewModel>>(replies); 

            return Ok(repliesPage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var reply = await _mediator.Send(new GetReplyByIdQuery() { Id = id });

            var replyViewModel = _mapper.Map<PublicReplyViewModel>(reply);

            return Ok(replyViewModel);
        }

        private void InitializeRequestProperties()
        {
            _organization = HttpContext.GetValueFromHeader("role");

            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
        }
    }
}
