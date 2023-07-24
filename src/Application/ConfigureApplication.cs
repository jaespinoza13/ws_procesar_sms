
using MediatR;
using System.Reflection;
using FluentValidation;
using Application.Common.Behaviours;
using Application.Common.ISO20022.Models;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //SERVICIOS
        services.AddValidatorsFromAssembly( Assembly.GetExecutingAssembly() );
        services.AddValidatorsFromAssemblyContaining<Header>();
        services.AddMediatR( Assembly.GetExecutingAssembly() );
        services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( ValidationBehaviour<,> ) );
        
        //CASOS DE USO
        return services;
    }
}