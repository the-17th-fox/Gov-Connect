using AutoMapper;
using Communications.Api.ViewModels.ElasticSearch;
using Communications.Application.ViewModels.ElasticSearch;

namespace Communications.Api.ViewModels.MapperProfiles;

public class ApiElasticSearchMapperProfile : Profile
{
    public ApiElasticSearchMapperProfile()
    {
        CreateMap<IndexedMessageViewModel, SearchResponseViewModel>();
    }
}