using Application.Common.ISO20022.Models;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace WebUI.Filters
{
    public class ClaimControlFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!validarClaims( context ))
            {
                ResException resException = new();
                resException.str_res_codigo = Convert.ToInt32( HttpStatusCode.Unauthorized ).ToString();
                resException.str_res_id_servidor = "Sesion Caducada";
                resException.str_res_estado_transaccion = "ERR";
                resException.dt_res_fecha_msj_crea = DateTime.Now;
                resException.str_res_info_adicional = "Tu sesión ha caducado, por favor ingresa nuevamente.";

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Result = new ObjectResult( resException );
            }
        }


        private bool validarClaims(ActionExecutingContext context)
        {
            var blValidacion = false;
            context.HttpContext.Request.EnableBuffering();
            var user = context.HttpContext.User;
            var claimRol = user.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Role );
            var model = context.ActionArguments.First();
            var header = JsonSerializer.Deserialize<Header>( JsonSerializer.Serialize( model.Value ) )!;

            if (claimRol != null && (
                claimRol!.Value.Equals( Rol.Socio ) ||
                claimRol!.Value.Equals( Rol.Usuario )) ||
                claimRol!.Value.Equals( Rol.InvitadoExterno )
                )
            {
                var claimEnte = user.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier );
                var claimCanal = user.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Name );

                blValidacion = (header.str_ente.Equals( claimEnte!.Value ) &&
                   header.str_nemonico_canal.Equals( claimCanal!.Value )) ? true : false;


            }

            else if (claimRol != null && claimRol!.Value.Equals( Rol.InvitadoInterno ))
            {
                var claimLogin = user.Claims.FirstOrDefault( c => c.Type == ClaimTypes.NameIdentifier );
                var claimCanal = user.Claims.FirstOrDefault( c => c.Type == ClaimTypes.Name );

                blValidacion = (header.str_login.Equals( claimLogin!.Value ) &&
                    header.str_nemonico_canal.Equals( claimCanal!.Value )) ? true : false;
            }


            return blValidacion;

        }

    }
}
