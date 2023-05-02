using Communications.Application;
using Communications.Application.AutoMapper;
using System.Reflection;

namespace Communications.Api.Configuration;

internal static class UtilitiesConfiguration
{
    internal static void ConfigureUtilities(this IServiceCollection services)
    {
        services.AddAutoMapper(opt =>
        {
            opt.AddMaps(Assembly.GetExecutingAssembly());

            opt.AddProfile<ApplicationMapperProfile>();
        });

        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(typeof(HandlerBase<>).Assembly);
        });
    }
}