using AutoMapper;
using Communications.Api.Utilities;
using Communications.Api.ViewModels.Notifications;
using Communications.Application.Notifications.Commands;
using Communications.SignalR.Extensions;
using Communications.SignalR.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Communications.Api.Controllers.Notifications
{
    [Route("api/notifications/authorities")]
    [ApiController]
    public class AuthoritiesNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        private readonly IHubContext<NotificationsHub> _notificationsHubContext;

        private string _organization = string.Empty;
        private Guid _userId;

        public AuthoritiesNotificationsController(
            IMediator mediator, 
            IMapper mapper, 
            IHubContext<NotificationsHub> notificationsHubContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _notificationsHubContext = notificationsHubContext ?? throw new ArgumentNullException(nameof(notificationsHubContext));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateNotificationViewModel createNotificationViewModel)
        {
            InitializeRequestProperties();

            var createNotificationCommand = _mapper.Map<CreateNotificationCommand>(createNotificationViewModel);
            createNotificationCommand.Organization = _organization;
            createNotificationCommand.AuthorityId = _userId;

            await _mediator.Send(createNotificationCommand);

            await SendReplyToGroupAsync(createNotificationCommand);

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

        private void InitializeRequestProperties()
        {
            _organization = HttpContext.GetValueFromHeader("role");

            _userId = Guid.Parse(HttpContext.GetValueFromHeader("uid"));
        }

        private async Task SendReplyToGroupAsync(CreateNotificationCommand notification)
        {
            await _notificationsHubContext.SendUpdateToGroup(
                groupName: NotificationsHub.GroupName,
                method: BaseCommunicationsHub.NewMessageMethodName,
                message: notification);
        }
    }
}
