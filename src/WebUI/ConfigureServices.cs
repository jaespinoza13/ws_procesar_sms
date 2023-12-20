using Application.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using System.Text;
using WebUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services, IConfiguration configuration)
    {
        // CUSTOMISE API EXCEPTIONS BEHAVIOUR
        services.AddControllersWithViews( options => options.Filters.Add<ApiExceptionFilterAttribute>() )
                .AddFluentValidation( x => x.RegisterValidatorsFromAssembly( Assembly.GetExecutingAssembly() ) );

        // CUSTOMISE DEFAULT API BEHAVIOUR
        services.Configure<ApiBehaviorOptions>( options => options.SuppressModelStateInvalidFilter = true );

        //CORS
        services.AddCors( options =>
        {
            options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.WithOrigins().WithMethods( "POST" ).AllowAnyHeader();
                    } );
        } );

        //AUTHORIZATION 
        var issuer = configuration.GetValue<string>( "issuer" );
        var publicKeyBytes = Convert.FromBase64String( configuration.GetValue<string>( "Key_token_pub" ) );
        RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo( publicKeyBytes, out _ );
        var keyRSA = new RsaSecurityKey( rsa );

        var securityKeyDecrypt = new SymmetricSecurityKey( Encoding.Default.GetBytes( configuration.GetValue<string>( "Key_encrypt_token" ) ) );


        services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                        .AddJwtBearer( opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issuer,
                            IssuerSigningKey = keyRSA,
                            TokenDecryptionKey = securityKeyDecrypt,
                            ClockSkew = TimeSpan.Zero
                        } );


        //SERVICES
        services.AddDataProtection();
        services.AddMemoryCache();
        services.AddOptions();


        //FILTERS
        services.AddTransient<DailyRequestFilter>();
        services.AddTransient<CryptographyAESFilter>();
        services.AddTransient<ClaimControlFilter>();
        services.AddTransient<SessionControlFilter>();

        //SWAGGER
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen( c =>
        {
            //Modificar
            c.SwaggerDoc( "v1", new OpenApiInfo
            {
                Title = "API Procesar SMS", 
                Version = "v1",
                Description = "servicio para procesar respuestas de socios por SMS"
            } );
        } );

        //CONFIGURATIONS
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:DataBases" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:GrpcSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:ConfigMongodb" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:LogsPath" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:LoadParameters" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:valida_peticiones_diarias" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:HttpSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:Endpoints" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:EndpointsAuth" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:AlfrescoSettings" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:Transferencia" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:DescripcionErrores" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:ControlExcepciones" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:TemplatesPath" ) );
        services.Configure<ApiSettings>( configuration.GetSection( "ApiSettings:LogosPath" ) );

        return services;
    }
}