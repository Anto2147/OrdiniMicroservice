using Ordini.ClientHttp;
using Ordini.ClientHttp.Abstraction;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class OrdiniClientExtensions
{
    public static IServiceCollection AddOrdiniClient(this IServiceCollection services, IConfiguration configuration)
    {

        IConfigurationSection confSection = configuration.GetSection(OrdiniClientOptions.SectionName);
        OrdiniClientOptions options = confSection.Get<OrdiniClientOptions>() ?? new();

        services.AddHttpClient<IOrdiniClient, OrdiniClient>(o => {
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

        return services;
    }
}


