using AutoMapper;
using Communications.Application.Notifications.Commands;
using Communications.Application.Replies.Commands;
using Communications.Application.Reports.Commands;
using Communications.Core.Models;

namespace Communications.Application.AutoMapper;

public class ApplicationMapperProfile : Profile
{
    public ApplicationMapperProfile()
    {
        CreateMap<CreateNotificationCommand, Notification>();

        CreateMap<CreateReplyCommand, Reply>();

        CreateMap<CreateReportCommand, Report>();
    }
}