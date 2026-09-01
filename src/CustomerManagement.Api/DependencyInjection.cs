using CustomerManagement.Api.Exceptions;
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

        var frontendUrl = configuration["CustomerManagementWebUrl"] ??
            throw new ArgumentNullException(nameof(configuration));

        services.AddCors(options =>
            options.AddPolicy("AngularDev", policy => policy
            .WithOrigins(frontendUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()));

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}
