using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace SharedLib.ElasticSearch.Extensions
{
    public static class ElasticSearchServiceConfiguration
    {
        public static IServiceCollection ConfigureElasticSearchClient(this IServiceCollection services, string uri)
        {
            var pool = new SingleNodeConnectionPool(new Uri(uri));

            var settings = new ConnectionSettings(pool);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            return services;
        }
    }
}