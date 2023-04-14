using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebUI.Filters
{
    public class SessionControlFilter : IActionFilter
    {
        private readonly ISessionControl _session;
        public SessionControlFilter(ISessionControl sessionControl) => _session = sessionControl;

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int estadoSesion = Convert.ToInt32( context.HttpContext.Request.Headers["int_estado"] );
            var model = context.ActionArguments.First();
            var raw = JsonSerializer.Deserialize<ValidaSesion>( JsonSerializer.Serialize( model.Value ) )!;
            raw.int_estado = estadoSesion;
            var respuesta = _session.SessionControlFilter( raw );

            if (!respuesta.codigo.Equals( "000" ))
            {
                ResException resException = new();
                resException.str_res_codigo = respuesta.codigo;
                resException.str_res_id_servidor = "Sesion Caducada";
                resException.str_res_estado_transaccion = "ERR";
                resException.dt_res_fecha_msj_crea = DateTime.Now;
                resException.str_res_info_adicional = respuesta.diccionario["str_error"];

                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                context.Result = new ObjectResult( resException );
            }
        }


    }
}
