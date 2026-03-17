using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Ecommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        //1.Validaciones
        services.AddValidatorsFromAssembly(assembly);
        //2.Mapeos
        services.AddAutoMapper(assembly);
        //3.MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(Common.Behaviors.ValidationBehavior<,>));
        });

        return services;
    }
}