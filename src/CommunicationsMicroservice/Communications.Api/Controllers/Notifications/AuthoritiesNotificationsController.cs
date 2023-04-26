using AutoMapper;
using Communications.Api.Utilities;
using Communications.Api.ViewModels.Notifications;
using Communications.Application.Notifications.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Notifications
{
    [Route("api/notifications")]
    [ApiController]
    public class AuthoritiesNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private string _organization = string.Empty;
        private Guid _userId;

        public AuthoritiesNotificationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private void InitializeRequestProperties()
        {
            _organization = HttpContext.GetValueFromHeader("role");
            
            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateAsync(CreateNotificationViewModel createNotificationViewModel)
        {
            InitializeRequestProperties();

            var createNotificationCommand = _mapper.Map<CreateNotificationCommand>(createNotificationViewModel);
            createNotificationCommand.Organization = _organization;
            createNotificationCommand.AuthorityId = _userId;

            await _mediator.Send(createNotificationCommand);

            return Created("new", null);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateNotificationViewModel updateNotificationViewModel)
        {
            var updateNotificationCommand = _mapper.Map<UpdateNotificationCommand>(updateNotificationViewModel);
            updateNotificationCommand.Id = id;

            await _mediator.Send(updateNotificationCommand);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteNotificationCommand() { Id = id });

            return Ok();
        }
    }
}
