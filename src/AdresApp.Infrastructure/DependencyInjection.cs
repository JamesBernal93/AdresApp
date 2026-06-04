using AdresApp.Application.Interfaces;
using AdresApp.Domain.Interfaces;
using AdresApp.Infrastructure.Auth;
using AdresApp.Infrastructure.External;
using AdresApp.Infrastructure.Persistence;
using AdresApp.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdresApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)));

        services.AddScoped<ICargueArchivoRepository, CargueArchivoRepository>();
        services.AddScoped<IAuthService, JwtAuthService>();

        services.AddHttpClient<IAdresExternalService, AdresExternalService>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(120);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        return services;
    }
}
