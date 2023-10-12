using AutoMapper;
using Communications.Api.ViewModels.Notifications;
using Communications.Api.ViewModels.Pagination;
using Communications.Application.Notifications.Commands;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Models;
using Communications.Infrastructure.Utilities;

namespace Communications.Api.ViewModels.MapperProfiles;

public class ApiNotificationsMapperProfile : Profile
{
    public ApiNotificationsMapperProfile()
    {
        // Model > ViewModel
        CreateMap<Notification, NotificationViewModel>();
        
        CreateMap<Notification, ShortNotificationViewModel>();
        
        CreateMap<IndexedMessageViewModel, ShortNotificationViewModel>()
            .ForMember(m => m.CreatedAt, opt => opt.MapFrom(src => src.Date));

        CreateMap<PagedList<Notification>, PageViewModel<ShortNotificationViewModel>>()
            .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));

        // ViewModel > Command
        CreateMap<UpdateNotificationViewModel, UpdateNotificationCommand>();
        
        CreateMap<CreateNotificationViewModel, CreateNotificationCommand>();
    }
}