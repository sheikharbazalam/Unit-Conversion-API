using Microsoft.OpenApi.Models;

namespace UnitConverter.API.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Unit Converter API",
                Version = "v1",
                Description = "A RESTful API for converting values between units. Supports Length, Temperature, Weight, Volume, Speed and Area.",
                Contact = new OpenApiContact
                {
                    Name = "Unit Converter API",
                    Email = "engineering@example.com"
                }
            });
        });

        return services;
    }
}
