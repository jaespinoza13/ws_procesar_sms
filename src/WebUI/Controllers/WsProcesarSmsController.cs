using Application.Common.Models;

// Authorization
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;

//USE CASE
using Application.DatosSms.ProcesarSms;
using Application.DatosSms.ObtenerSms;

namespace WebUI.Controllers
{
    [Route( "api/servicio_procesar_sms" )]
    [ApiController]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( StatusCodes.Status400BadRequest )]
    [ProducesResponseType( StatusCodes.Status401Unauthorized )]
    [ProducesResponseType( StatusCodes.Status500InternalServerError )]
    public class WsProcesarSmsController : ApiControllerBase
    {
        private readonly ApiSettings _settings;
        public WsProcesarSmsController(IOptionsMonitor<ApiSettings> options) => _settings = options.CurrentValue;

        // POST: api/servicio_procesar_sms/OBTENER_SMS
        // Para uso exclusivo de eclipsoft
        [HttpPost( "OBTENER_SMS" )]
        [Produces( "application/json" )]
        public async Task<ResObtenerSms> ObtenerSms(ReqObtenerSms reqObtenerSms)
        {
                    return await Mediator.Send( reqObtenerSms );
        }

        // POST: api/servicio_procesar_sms/PROCESAR_SMS
        // Para uso del servicio windows
        [HttpPost( "PROCESAR_SMS" )]
        [Produces( "application/json" )]
        public async Task<ResProcesarSms> ProcesarSms(ReqProcesarSms reqProcesarSms)
        {
            return await Mediator.Send( reqProcesarSms );
        }
    }
}
