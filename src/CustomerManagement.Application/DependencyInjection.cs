using CustomerManagement.Application;
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
            cfg.LicenseKey = configuration["MediatRLicenseKey"];
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = configuration["AutoMapperLicenseKey"];
            cfg.AddMaps(typeof(IApplicationMarker).Assembly);
        });

        return services;
    }
}
