using Authorities.Api.ViewModels.Pagination;
using Authorities.Api.ViewModels.Users;
using Authorities.Core.Models;
using Authorities.Infrastructure.Utilities;
using AutoMapper;

namespace Authorities.Api.ViewModels
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
