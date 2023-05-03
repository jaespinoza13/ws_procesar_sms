using System.Text;
using System.Text.Json;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.ISO20022.Models;

namespace WebUI.Middleware
{
    public static class AuthorizationExtensions
    {
        public static IApplicationBuilder UseAuthotizationMego(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Authorization>();
        }
    }

    public class Authorization
    {
        private readonly RequestDelegate _next;
        private readonly ApiSettings _settings;
        private readonly IConfiguration _config;

        public Authorization(RequestDelegate next, IOptionsMonitor<ApiSettings> settings, IConfiguration config)
        {
            _next = next;
            _settings = settings.CurrentValue;
            _config = config;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var usuario = _settings.auth_ws_procesar_sms_user;
            var password = _settings.auth_ws_procesar_sms_pass;

            var headers = httpContext.Request.Headers;
            string authorization_header = Convert.ToString( headers["Authorization-Mego"] );

            if (authorization_header != null && authorization_header.StartsWith( "Auth-Mego" ))
            {
                string auth = authorization_header.Substring( "Auth-Mego ".Length ).Trim();

                var userPwd = Encoding.UTF8.GetString( Convert.FromBase64String( auth ) );
                var user = userPwd.Substring( 0, userPwd.IndexOf( ":" ) );
                var pass = userPwd.Substring( userPwd.IndexOf( ":" ) + 1 );


                if ((user.Equals( usuario ) && pass.Equals( password )))
                {
                    await _next( httpContext );
                }
                else
                {
                    await ResException( httpContext, "Credenciales erroneas", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
                }
            }
            else
            {
                await ResException( httpContext, "No autorizado", Convert.ToInt32( System.Net.HttpStatusCode.Unauthorized ), System.Net.HttpStatusCode.Unauthorized.ToString() );
            }
        }

        internal async Task ResException(HttpContext httpContext, String infoAdicional, int statusCode, string str_res_id_servidor)
        {
            ResException respuesta = new();

            httpContext.Response.ContentType = "application/json; charset=UTF-8";
            httpContext.Response.StatusCode = statusCode;

            respuesta.str_res_id_servidor = str_res_id_servidor;
            respuesta.str_res_codigo = "001";
            respuesta.dt_res_fecha_msj_crea = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null );
            respuesta.str_res_estado_transaccion = "ERR";
            respuesta.str_res_info_adicional = infoAdicional;

            string str_respuesta = JsonSerializer.Serialize( respuesta );

            await httpContext.Response.WriteAsync( str_respuesta );
        }
    }
}
