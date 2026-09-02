using CustomerManagement.Api.Exceptions;
using Microsoft.OpenApi;
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

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerManagement", Version = "v1" });
            c.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
            {
                Description = "Enter your API Key.",
                Name = "X-API-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("apiKey", document), new List<string>() }
            });
        });

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddControllers();

        return services;
    }
}
