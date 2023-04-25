using AutoMapper;
using Civilians.Api.ViewModels.Pagination;
using Civilians.Api.ViewModels.Passports;
using Civilians.Api.ViewModels.Users;
using Civilians.Core.Models;
using Civilians.Infrastructure.Utilities;

namespace Civilians.Api.ViewModels
{
    public class ApiMapperProfile : Profile
    {
        public ApiMapperProfile()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<User, ShortUserViewModel>();

            CreateMap<PagedList<User>, PageViewModel<ShortUserViewModel>>()
                .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));

            CreateMap<Passport, PassportViewModel>();
        }
    }
}
