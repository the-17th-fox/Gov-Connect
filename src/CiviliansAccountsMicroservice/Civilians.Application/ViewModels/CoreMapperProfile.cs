using AutoMapper;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Models;

namespace Civilians.Application.ViewModels
{
    public class CoreMapperProfile : Profile
    {
        public CoreMapperProfile() 
        {
            CreateMap<UpdatePassportViewModel, Passport>();
        }
    }
}
