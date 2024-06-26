using MediatR;
using System.Reflection;
using Application.Interfaz;
using Application.SmsSocio;
using Application.Common.Models;
using Application.Common.Interfaces;
using Application.SmsSocio.ValidarSms;
using Application.SmsSocio.ProcesarTransf;
using Application.SmsSocio.SmsPorProcesar;

namespace Application.DatosSms.ProcesarSms
{
    public class ProcesarSmsHandler : IRequestHandler<ReqProcesarSms, ResProcesarSms>
    {
        public readonly ILogs _logsService;
        private readonly string str_clase;
        private readonly IMensajesDat _mensaje;
        
        public ProcesarSmsHandler(ILogs logsService, IMensajesDat mensaje)
        {
            _logsService = logsService;
            str_clase = GetType().Name;
            _mensaje = mensaje;
        }

        public async Task<ResProcesarSms> Handle(ReqProcesarSms request, CancellationToken cancellationToken)
        {
            List<SmsProcesado> list = new List<SmsProcesado>();
            string strOperacion = "PROCESAR_SMS";
            int int_sms_id = -1;

            ResProcesarSms proveedor_response = new ResProcesarSms();
            proveedor_response.LlenarResHeader( request );
            SmsServices sms = new SmsServices( _logsService, _mensaje, proveedor_response.str_id_transaccion );

             _ = _logsService.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
            
            try
            {
                ResSmsPorProcesar sms_list = await sms.GetSmsPorProcesar();

                if(sms_list != null )
                {
                    if(sms_list.sms_procesar.Count > 0)
                    {

                        await Parallel.ForEachAsync( sms_list.sms_procesar, async (item_sms, _) =>
                        {
                            int_sms_id = item_sms.int_sms_id;
                            ReqValidarSms req_valid_sms = new ReqValidarSms();
                            req_valid_sms.int_sms_id = item_sms.int_sms_id;
                            var response_sms = await sms.ValidarSms( req_valid_sms, request.str_login, request.str_ip_dispositivo );
                            await response_method( request, item_sms, list, sms, response_sms );

                        } );
                    } else
                    {
                        proveedor_response.str_res_codigo = "006";
                        proveedor_response.str_res_info_adicional = "No se encontraron SMS para procesar";
                    }
                }
                else
                {
                    proveedor_response.str_res_codigo = "006";
                    proveedor_response.str_res_info_adicional = "No se encontraron SMS para procesar";
                }
                proveedor_response.sms_procesados = list;
                 _ = _logsService.SaveResponseLogs( proveedor_response, strOperacion, MethodBase.GetCurrentMethod()!.Name, str_clase );
                return proveedor_response;
            }
            catch (Exception exception)
            {
                   _ = sms.ActualizarEstadoSms( int_sms_id, "EPS_PROCESADO_ERROR", request.str_id_usuario, request.str_ip_dispositivo );
                  _ = _logsService.SaveExceptionLogs( proveedor_response, strOperacion, MethodBase.GetCurrentMethod()!.Name, str_clase, exception );
                throw new ArgumentException( "Error" )!;
            }
        }

        private static async Task response_method(ReqProcesarSms request, SmsProcesar item_sms, List<SmsProcesado> list, SmsServices sms, ResValidarSms response_sms)
        {
            if (response_sms.str_res_estado_transaccion == "OK")
            {

                ReqProcesarTransf req_procs_transf = new ReqProcesarTransf
                {
                    str_num_telefono = item_sms.str_telefono,
                    str_fecha_transaccion = item_sms.str_fecha_recepcion,
                    int_sms_id = item_sms.int_sms_id
                };

                var resp_proces_transf = await sms.ProcesarTransferencia( req_procs_transf, request.str_ip_dispositivo );

                if (resp_proces_transf.str_res_estado_transaccion == "OK")
                {
                     _ = sms.ActualizarEstadoSms( item_sms.int_sms_id, "EPS_PROCESADO_OK", request.str_login, request.str_ip_dispositivo );
                }
                else
                {
                     _ = sms.ActualizarEstadoSms( item_sms.int_sms_id, "EPS_PROCESADO_ERROR", request.str_login, request.str_ip_dispositivo );
                }
                list.Add( new SmsProcesado { codigo = resp_proces_transf.str_res_codigo, mensaje = $"El Sms con ID. {item_sms.int_sms_id} ha sido procesado." } );
            }
            else
            {
                 _ = sms.ActualizarEstadoSms( item_sms.int_sms_id, "EPS_PROCESADO_ERROR", request.str_login, request.str_ip_dispositivo );
                list.Add( new SmsProcesado { codigo = "005", mensaje = $"El Sms con ID. {item_sms.int_sms_id} no contiene palabras clave." } );
            }
        }
    }
}
