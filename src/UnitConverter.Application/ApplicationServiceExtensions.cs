using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using UnitConverter.Application.Services;
using UnitConverter.Application.Validators;

namespace UnitConverter.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ConversionService>();
        services.AddScoped<UnitCatalogueService>();
        services.AddScoped<HistoryService>();
        services.AddValidatorsFromAssemblyContaining<ConversionRequestValidator>();
        return services;
    }
}
