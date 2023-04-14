using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Application.Interfaz;
using Application.SmsSocio.ValidarCodigoSms;
using Application.SmsSocio.ValidarPalabraClave;
using Application.SmsSocio.ValidarSms;
using Domain.Entities;
using System.Reflection;

namespace Application.SmsSocio
{
    public class SmsServices
    {
        public readonly ILogs _logsService;
        private readonly string _clase;
        private readonly IMensajesDat _sms;

        public SmsServices(ILogs logsService, IMensajesDat sms)
        {
            _logsService = logsService;
            _clase = GetType().Name;
            _sms = sms;
        }

        public async Task<ResValidarCodigoSms> ValidarCodigo(int int_codigo_sms)
        {
            ResValidarCodigoSms res_val = new();
            RespuestaTransaccion res_tran = await _sms.ValidarCodigoSms( int_codigo_sms );

            if (res_tran.codigo.Equals( "000" ))
            {
                res_val = Conversions.ConvertConjuntoDatosToClass<ResValidarCodigoSms>( (ConjuntoDatos)res_tran.cuerpo )!;
            }

            return res_val;
        }

        public async Task<ResValidarSms> ValidarSms(ReqValidarSms reqValidarSms)
        {
            string strOperacion = "VALIDAR_SMS";
            ResValidarSms respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ValidarSms( reqValidarSms );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                if (res_tran.codigo.Equals( "000" ))
                {
                    respuesta.palabras_clave = Conversions.ConvertConjuntoDatosToListClass<PalabrasSms>( (ConjuntoDatos)res_tran.cuerpo );
                }
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException();
            }
        }

        public async Task<ResValidarPalabraClave> ValidarPalabraClave(string str_texto_sms)
        {
            string strOperacion = "VALIDAR_PALABRA_CLAVE";
            ResValidarPalabraClave respuesta = new();
            try
            {
                var res_tran = await _sms.ValidarPalabraClaveBloquear( str_texto_sms );
                respuesta.str_res_estado_transaccion = res_tran.codigo.Equals( "000" ) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;

                respuesta.palabras_clave = Conversions.ConvertConjuntoDatosToListClass<PalabrasSms>( (ConjuntoDatos)res_tran.cuerpo );
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;

            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException( respuesta.str_id_transaccion );
            }
        }

        public async Task<ResComun> ProcesarTransferencia(string str_num_telefono, string str_fecha_transaccion, int int_sms_id)
        {
            string strOperacion = "PROCESAR_TRANSFERENCIA";
            ResComun respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ProcesarTransferencia( str_num_telefono, str_fecha_transaccion, int_sms_id );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException();
            }
        }

        public async void GuardarSmsSocio(Sms sms, string str_sms_estado)
        {
            string strOperacion = "GUARDAR_SMS_SOCIO";
            ResComun respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.GuardarSmsSocio( sms, str_sms_estado );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException();
            }
        }

        public async void ActualizarEstadoSms(int int_sms_id, string str_estado_sms)
        {
            string strOperacion = "ACTUALIZAR_ESTADO_SMS";
            ResComun respuesta = new();
            try
            {
                RespuestaTransaccion res_tran = await _sms.ActualizarEstadoProcesoSms( int_sms_id, str_estado_sms );
                respuesta.str_res_estado_transaccion = (res_tran.codigo.Equals( "000" )) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;
                respuesta.str_res_info_adicional = res_tran.diccionario["str_error"].ToString();
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException();
            }
        }
    }
}
