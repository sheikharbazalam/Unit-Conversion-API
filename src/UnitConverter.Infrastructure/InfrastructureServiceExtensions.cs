using Microsoft.Extensions.DependencyInjection;
using UnitConverter.Application.Interfaces;
using UnitConverter.Infrastructure.Repositories;

namespace UnitConverter.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitRepository, InMemoryUnitRepository>();
        services.AddSingleton<IConversionHistoryRepository, InMemoryConversionHistoryRepository>();
        return services;
    }
}
