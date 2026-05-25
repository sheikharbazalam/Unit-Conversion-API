using Serilog;
using Serilog.Events;
using UnitConverter.API.Extensions;
using UnitConverter.API.Middleware;
using UnitConverter.Application;
using UnitConverter.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/unit-converter-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    Log.Information("Starting Unit Converter API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services
        .AddApplicationServices()
        .AddInfrastructureServices()
        .AddControllers();

    builder.Services
        .AddSwaggerDocumentation()
        .AddApiRateLimiting()
        .AddResponseCaching()
        .AddCors(opts =>
            opts.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unit Converter API v1");
        c.RoutePrefix = string.Empty;
    });

    app.UseCors();
    app.UseRateLimiter();
    app.UseResponseCaching();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
