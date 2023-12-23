using Domain.Entities;
using System.Reflection;
using Application.Interfaz;
using Application.Common.Models;
using Application.Common.Functions;
using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.SmsSocio.ValidarSms;
using Application.SmsSocio.ProcesarTransf;
using Application.SmsSocio.ValidarPalabraClave;
using Application.SmsSocio.AgregarSms;
using Application.SmsSocio.SmsPorProcesar;
using Org.BouncyCastle.Asn1.Ocsp;
using MediatR;

namespace Application.SmsSocio
{
    public class SmsServices
    {
        public readonly ILogs _logsService;
        private readonly string _clase;
        private readonly IMensajesDat _sms;
        private readonly string _transaccion;

        public SmsServices(ILogs logsService, IMensajesDat sms, string transaccion)
        {
            _logsService = logsService;
            _clase = GetType().Name;
            _sms = sms;
            _transaccion= transaccion;
        }

        public async Task<ResSmsPorProcesar> GetSmsPorProcesar()
        {
            string strOperacion = "GET_SMS_POR_PROCESAR";
            var log_body = new LogBody
            {
                str_id_transaccion = _transaccion,
                dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null ),
                str_operacion = strOperacion
            };

            await _logsService.SaveHeaderLogs( log_body, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            ResSmsPorProcesar res_val = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.GetSmsPorProcesar();

                if (res_tran.codigo.Equals( "000" ))
                {
                    res_val.sms_procesar = Conversions.ConvertConjuntoDatosToListClass<SmsProcesar>( (ConjuntoDatos)res_tran.cuerpo )!;
                }
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( log_body.Spread( res_val ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException("Error al obtener los sms por procesar.");
            }
            await _logsService.SaveResponseLogs( log_body.Spread( res_val ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            return res_val;
        }

        public async Task<ResValidarSms> ValidarSms(ReqValidarSms reqValidarSms, string str_login, string str_ip_dispositivo)
        {
            string strOperacion = "VALIDAR_SMS";
            var log_body = new LogBody
            {
                str_id_transaccion = _transaccion,
                dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null ),
                str_operacion = strOperacion
            };

            await _logsService.SaveHeaderLogs( log_body.Spread( reqValidarSms ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            ResValidarSms respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ValidarSms( reqValidarSms, str_login, str_ip_dispositivo );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                if (res_tran.codigo.Equals( "000" ))
                {
                    respuesta.palabras_clave = Conversions.ConvertConjuntoDatosToListClass<PalabrasSms>( (ConjuntoDatos)res_tran.cuerpo );
                }
                await _logsService.SaveResponseLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException( "Error al validar el sms.");
            }
        }

       
        public async Task<ResProcesarTransf> ProcesarTransferencia(ReqProcesarTransf req_procesar_transf, string str_ip_dispositivo)
        {
            string strOperacion = "PROCESAR_TRANSFERENCIA";
            var log_body = new LogBody
            {
                str_id_transaccion = _transaccion,
                dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null ),
                str_operacion = strOperacion
            };

            await _logsService.SaveHeaderLogs( log_body.Spread( req_procesar_transf ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            ResProcesarTransf respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ProcesarTransferencia( req_procesar_transf, str_ip_dispositivo );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                await _logsService.SaveResponseLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException( "Error al procesar las transferencias." );
            }
        }

        public async Task<ResAgregarSms> GuardarSmsSocio(ReqAgregarSms req_agregar_sms)
        {
            string strOperacion = "GUARDAR_SMS_SOCIO";
            var log_body = new LogBody
            {
                str_id_transaccion = _transaccion,
                dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null ),
                str_operacion = strOperacion
            };

            await _logsService.SaveHeaderLogs( log_body.Spread( req_agregar_sms.sms, new { estado = req_agregar_sms.str_sms_estado } ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            ResAgregarSms respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.GuardarSmsSocio( req_agregar_sms.sms, req_agregar_sms.str_sms_estado );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                respuesta.int_duplicado_sms = Convert.ToInt32(res_tran.diccionario["int_duplicado"]);
                await _logsService.SaveResponseLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException( "Error al almacenar el sms." );
            }
            return respuesta;
        }

        public async Task ActualizarEstadoSms(int int_sms_id, string str_estado_sms, string str_login, string str_ip_dispositivo)
        {
            string strOperacion = "ACTUALIZAR_ESTADO_SMS";
            var log_body = new LogBody
            {
                str_id_transaccion = _transaccion,
                dt_fecha_operacion = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null ),
                str_operacion = strOperacion
            };

            await _logsService.SaveResponseLogs( log_body.Spread( new { sms_id = int_sms_id, estado = str_estado_sms }), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            ResProcesarTransf respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ActualizarEstadoProcesoSms( int_sms_id, str_estado_sms, str_login, str_ip_dispositivo );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                await _logsService.SaveResponseLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( log_body.Spread( respuesta ), strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException( "Error al actualizar el estado del sms." );
            }
        }
    }
}
