using AutoMapper;
using Communications.Api.ViewModels.Pagination;
using Communications.Api.ViewModels.Reports;
using Communications.Application.BaseMethods;
using Communications.Application.Reports.Commands;
using Communications.Application.Reports.Queries;
using Communications.Core.Models;
using Communications.Infrastructure.Utilities;

namespace Communications.Api.ViewModels.MapperProfiles;

public class ApiReportsMapperProfile : Profile
{
    public ApiReportsMapperProfile()
    {
        // Model > ViewModel
        CreateMap<Report, PublicReportViewModel>();
        CreateMap<Report, ShortReportViewModel>();
        CreateMap<PagedList<Report>, PageViewModel<ShortReportViewModel>>()
            .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));

        // ViewModel > Command
        CreateMap<BaseGetAllQuery<Report>, GetAllReportsByCivilianQuery>();
        CreateMap<CreateReportViewModel, CreateReportCommand>();
    }
}