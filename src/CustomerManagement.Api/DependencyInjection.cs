using CustomerManagement.Api.Middleware;
using Serilog;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((sp, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(sp)
            .WriteTo.Console());

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}
