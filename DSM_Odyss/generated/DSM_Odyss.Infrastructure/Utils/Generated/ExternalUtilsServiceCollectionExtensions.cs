using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DSM_Odyss.Application.Abstractions;
using DSM_Odyss.Infrastructure.Utils.Manual;

namespace DSM_Odyss.Infrastructure.Utils.Generated;

public static class ExternalUtilsServiceCollectionExtensions
{
    public static IServiceCollection AddHariaExternalUtils(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWorkUtils, UnitOfWorkUtils>();
        ExternalUtilRegistration.Register(services, configuration);
        return services;
    }
}