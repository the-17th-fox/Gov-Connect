using AutoMapper;
using Civilians.Application.ViewModels.Passports;
using Civilians.Core.Models;

namespace Civilians.Application.ViewModels
{
    public class ApplicationMapperProfile : Profile
    {
        public ApplicationMapperProfile() 
        {
            CreateMap<UpdatePassportViewModel, Passport>();
        }
    }
}
