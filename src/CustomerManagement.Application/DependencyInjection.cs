using CustomerManagement.Application;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(IApplicationMarker).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.LicenseKey = configuration["MediatRLicenseKey"];
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = configuration["AutoMapperLicenseKey"];
            cfg.AddMaps(typeof(IApplicationMarker).Assembly);
        });

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
