using AutoMapper;
using Communications.Api.ViewModels.Classifications;
using Communications.Api.ViewModels.Pagination;
using Communications.Core.Models;
using Communications.Infrastructure.Utilities;

namespace Communications.Api.ViewModels.MapperProfiles;

public class ApiClassificationsMapperProfile : Profile
{
    public ApiClassificationsMapperProfile()
    {
        CreateMap<Classification, ClassificationViewModel>();
        CreateMap<PagedList<Classification>, PageViewModel<ClassificationViewModel>>()
            .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));
    }
}