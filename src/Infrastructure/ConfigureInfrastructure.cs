using Application.Interfaz;
using Infrastructure.Services;
using Infrastructure.MemoryCache;
using Infrastructure.DailyRequest;
using Infrastructure.SessionControl;
using Application.Common.Interfaces;
using Infrastructure.Common.Interfaces;
using Infrastructure.gGRPC_Clients.Mongo;
using Infrastructure.gRPC_Clients.Sybase;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        //INTERFACES DE SERVICIOS
        services.AddSingleton<ParametersInMemory>();
        services.AddSingleton<ILogs, LogsService>();
        services.AddSingleton<IMongoDat, LogsMongoDat>();
        services.AddSingleton<IDailyRequest, DailyRequest>();
        services.AddSingleton<IParametersInMemory, ParametersInMemory>();
        services.AddSingleton<IParametrosDat, ParametrosDat>();
        services.AddTransient<IHttpService, HttpService>();
        services.AddTransient<ISqlInjectionValidationService, SqlInjectionValidationService>();
        services.AddSingleton<IOtpDat, OtpDat>();
        services.AddTransient<ISessionControl, SessionControl>();
        services.AddSingleton<ISesionDat, SesionDat>();
        services.AddSingleton<IKeysDat, KeysDat>();


        //INTERFACES DE CASOS DE USO
        services.AddSingleton<IMensajesDat, MensajesDat>();


        return services;
    }
}
