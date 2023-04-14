using AutoMapper;
using Civilians.Api.ViewModels.Users;
using Civilians.Core.Models;
using Civilians.Infrastructure.Utilities;
using SharedLib.Pagination.ViewModels;

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
        }
    }
}
