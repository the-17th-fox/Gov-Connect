using AutoMapper;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Models;

namespace Civilians.Application.ViewModels
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<UpdatePassportViewModel, Passport>();

            CreateMap<RefreshToken, RefreshToken>()
                .ForMember(refreshToken => refreshToken.Token, opt => opt.Ignore());
        }
    }
}
