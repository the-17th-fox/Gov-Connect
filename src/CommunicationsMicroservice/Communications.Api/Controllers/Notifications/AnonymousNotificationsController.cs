using AutoMapper;
using Communications.Api.ViewModels.Notifications;
using Communications.Api.ViewModels.Pagination;
using Communications.Application.Notifications.Queries;
using Communications.Application.Notifications.Queries.GetAllNotifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Communications.Api.Controllers.Notifications
{
    [Route("api/notifications/public")]
    [ApiController]
    public class AnonymousNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AnonymousNotificationsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var notification = await _mediator.Send(new GetNotificationByIdQuery() { Id = id });

            var notificationViewModel = _mapper.Map<NotificationViewModel>(notification);

            return Ok(notificationViewModel);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetAllAsync(GetAllNotificationsQuery getAllNotificationsQuery)
        {
            var notifications = await _mediator.Send(getAllNotificationsQuery);

            var notificationsPage = _mapper.Map<PageViewModel<ShortNotificationViewModel>>(notifications);

            return Ok(notificationsPage);
        }
    }
}
