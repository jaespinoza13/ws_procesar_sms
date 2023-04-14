using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Application.Interfaz;
using Application.SmsSocio;
using Application.SmsSocio.ValidarCodigoSms;
using Application.SmsSocio.ValidarSms;
using MediatR;
using System.Reflection;

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
            string strOperacion = "PROCESAR_SMS";
            SmsServices sms = new SmsServices( _logsService, _mensaje );
            ResProcesarSms proveedor_response = new ResProcesarSms();

            await _logsService.SaveHeaderLogs( request, strOperacion, MethodBase.GetCurrentMethod()!.Name, str_clase );

            int int_codigo_sms = request.sms.obj_mensaje.int_codigo;
            string str_num_telefono = request.sms.obj_mensaje.str_telefono;
            string str_fecha_recepcion = request.sms.obj_mensaje.str_srv_hora_rec;

            try
            {
                ResValidarCodigoSms res_cod = await sms.ValidarCodigo( int_codigo_sms );
                int validez_codigo_sms = res_cod.validez;

                if (validez_codigo_sms == 1 )
                {
                    ReqValidarSms request_sms = new();
                    ResValidarSms response_sms = new();
                    request_sms.int_codigo = request.sms.obj_mensaje.int_codigo;
                    request_sms.str_codigo_raiz = request.sms.str_codigo_sms_raiz;
                    request_sms.str_telefono = request.sms.obj_mensaje.str_telefono;
                    request_sms.str_texto_sms = request.sms.obj_mensaje.str_texto;
                    request_sms.str_fecha_recepcion = request.sms.obj_mensaje.str_srv_hora_rec;
                    request_sms.str_observacion = request.sms.obj_mensaje.str_observacion;
                    request_sms.str_operadora = request.sms.str_operadora;
                    request_sms.str_short_code = request.sms.str_short_Code;
                    request_sms.str_emisor = request.sms.str_emisor;
                    request_sms.str_estado_sms_proveedor = request.sms.obj_mensaje.str_estado;

                    response_sms = await sms.ValidarSms( request_sms );

                    if(response_sms.str_res_estado_transaccion == "OK")
                    {
                        ResComun resp_proces_transf = new(); 
                       
                        int int_sms_id = response_sms.palabras_clave[0].sms_id;
                        resp_proces_transf = await sms.ProcesarTransferencia( str_num_telefono, str_fecha_recepcion, int_sms_id );

                        if(resp_proces_transf.str_res_estado_transaccion == "OK")
                        {
                            sms.ActualizarEstadoSms( int_sms_id, "EPS_PROCESADO_OK" );
                        } else
                        {
                            sms.ActualizarEstadoSms( int_sms_id, "EPS_PROCESADO_ERROR" );
                        }
                        proveedor_response.codigo = "000";
                        proveedor_response.mensaje = "OK";
                    }
                    else
                    {
                        proveedor_response.codigo = response_sms.str_res_codigo;
                        proveedor_response.mensaje = response_sms.str_res_info_adicional;
                    }
                }
                else
                {
                    Sms sms_request = request.sms;
                    sms.GuardarSmsSocio(sms_request, "EPS_PROCESADO_ERROR" );

                    proveedor_response.codigo = "002";
                    proveedor_response.mensaje = "Codigo duplicado";
                }

                return proveedor_response;
            }
            catch (Exception exception)
            {
                await _logsService.SaveExceptionLogs( proveedor_response, strOperacion, MethodBase.GetCurrentMethod()!.Name, str_clase, exception );
                throw new ArgumentException( "Error" )!;
            }
        }
    }
}
