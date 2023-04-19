using Authorities.Api.ViewModels.Pagination;
using Authorities.Api.ViewModels.Roles;
using AutoMapper;
using Authorities.Api.ViewModels.Users;
using Authorities.Core.Models;
using Authorities.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;

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

            CreateMap<IdentityRole<Guid>, RoleViewModel>();

            CreateMap<PagedList<IdentityRole<Guid>>, PageViewModel<RoleViewModel>>()
                .ForMember(p => p.Items, opt => opt.MapFrom(src => src.ToList()));
        }
    }
}
