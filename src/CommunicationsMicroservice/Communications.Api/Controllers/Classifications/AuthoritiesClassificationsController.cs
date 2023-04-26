using AutoMapper;
using Communications.Api.ViewModels.Classifications;
using Communications.Application.Classifications.Commands;
using Communications.Application.Classifications.Commands.DeleteClassification;
using Communications.Application.Classifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Classifications
{
    [Route("api/classifications")]
    [ApiController]
    public class AuthoritiesClassificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AuthoritiesClassificationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var classification = await _mediator.Send(new GetClassificationByIdQuery() { Id = id });
            var classificationViewModel = _mapper.Map<ClassificationViewModel>(classification);

            return Ok(classificationViewModel);
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync(CreateClassificationCommand createClassificationCommand)
        {
            await _mediator.Send(createClassificationCommand);

            return Created("new", null);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] string newName)
        {
            await _mediator.Send(new UpdateClassificationCommand() { Id = id, NewName = newName });

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteClassificationCommand() { Id = id });

            return Ok();
        }
    }
}
