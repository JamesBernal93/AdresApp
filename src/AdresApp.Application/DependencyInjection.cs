using AdresApp.Application.Interfaces;
using AdresApp.Application.Services;
using AdresApp.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AdresApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICargueArchivoService, CargueArchivoService>();
        //object value = services.AddValidatorsFromAssemblyContaining<CargueArchivoRequestValidator>();
        return services;
    }
}
