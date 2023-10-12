using Ocelot.DependencyInjection;

namespace Gateway.Configuration;

public static class OcelotConfiguration
{
    public static void ConfigureOcelot(this IServiceCollection services, ConfigurationManager configuration)
    {
        configuration.AddJsonFile("ocelot.authorities.json", optional: false, reloadOnChange: true);
        configuration.AddJsonFile("ocelot.civilians.json", optional: false, reloadOnChange: true);
        configuration.AddJsonFile("ocelot.communications.json", optional: false, reloadOnChange: true);

        configuration.AddJsonFile("docker.authorities.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile("docker.civilians.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile("docker.communications.json", optional: true, reloadOnChange: true);

        services.AddOcelot(configuration);
    }
}