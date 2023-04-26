using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Replies;
using Communications.Application.Replies.Commands;
using Communications.Core.Models;
using Communications.Infrastructure.Utilities;

namespace Communications.Api.ViewModels.MapperProfiles;

public class ApiRepliesMapperProfile : Profile
{
    public ApiRepliesMapperProfile()
    {
        // Model > ViewModel
        CreateMap<Reply, ShortReplyViewModel>();
        CreateMap<Reply, PublicReplyViewModel>();
        CreateMap<PagedList<Reply>, PageViewModel<ShortReplyViewModel>>()
            .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));

        // ViewModel > Command
        CreateMap<CreateReplyViewModel, CreateReplyCommand>();
    }
}