using MediatR;
using System.Reflection;
using Application.SmsSocio;
using Application.Interfaz;
using Application.Common.Interfaces;
using Application.SmsSocio.AgregarSms;

namespace Application.DatosSms.ObtenerSms
{
    public class ObtenerSmsHandler : IRequestHandler<ReqObtenerSms, ResObtenerSms>
    {
        public readonly ILogs _logsService;
        private readonly string _clase;
        private readonly IMensajesDat _mensaje;

        public ObtenerSmsHandler(ILogs logsService, IMensajesDat mensaje)
        {
            _logsService = logsService;
            _clase = GetType().Name;
            _mensaje = mensaje;
        }

        public async Task<ResObtenerSms> Handle(ReqObtenerSms req_obtener_sms, CancellationToken cancellationToken)
        {
            string strOperacion = "OBTENER_SMS";
     
            ResObtenerSms proveedor_response = new ResObtenerSms();
            proveedor_response.LlenarResHeader( req_obtener_sms );

            // Instanciar la clase SmsServices --> Application/SmsSocio/SmsServices
            SmsServices sms = new SmsServices( _logsService, _mensaje, proveedor_response.str_id_transaccion );

            await _logsService.SaveHeaderLogs( req_obtener_sms, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );

            try
            {
                ReqAgregarSms req = new ReqAgregarSms
                {
                    sms = req_obtener_sms.sms,
                    str_sms_estado = "EPS_INGRESADO"
                };

                var response_add_sms = await sms.GuardarSmsSocio( req );

                proveedor_response.str_res_info_adicional = response_add_sms.str_res_info_adicional;
                proveedor_response.int_duplicado_sms = response_add_sms.int_duplicado_sms;
                proveedor_response.str_res_estado_transaccion = (response_add_sms.str_res_codigo.Equals( "000" )) ? "OK" : "ERR";
                proveedor_response.str_res_codigo = response_add_sms.str_res_codigo;
                proveedor_response.str_res_info_adicional = response_add_sms.str_res_info_adicional;

                return proveedor_response;
            }
            catch (Exception exception)
            {
                await _logsService.SaveExceptionLogs( proveedor_response, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, exception );
                throw new ArgumentException( proveedor_response.str_id_transaccion );
            }
        }
    }
}
