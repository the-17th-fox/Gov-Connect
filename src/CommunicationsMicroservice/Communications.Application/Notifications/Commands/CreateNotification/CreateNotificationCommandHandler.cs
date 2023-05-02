using AutoMapper;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Notifications.Commands;

public class CreateNotificationCommandHandler : NotificationsHandlerBase, IRequestHandler<CreateNotificationCommand>
{
    private readonly IMapper _mapper;

    public CreateNotificationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = _mapper.Map<Notification>(request);

        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                notification.Classifications.Add(classification);
            }
        }

        UnitOfWork.NotificationsRepository.Create(notification);

        await UnitOfWork.SaveChangesAsync();
    }
}