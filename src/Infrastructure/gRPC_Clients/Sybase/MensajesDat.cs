using System.Reflection;
using Application.Interfaz;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using Infrastructure.Common.Funciones;
using Application.SmsSocio.ValidarSms;
using static AccesoDatosGrpcAse.Neg.DAL;
using Application.SmsSocio.ProcesarTransf;

namespace Infrastructure.gRPC_Clients.Sybase
{
    public class MensajesDat : IMensajesDat
    {
        public readonly ApiSettings _settings;
        public readonly DALClient _objClienteDal;
        private readonly ILogs _logsService;
        public readonly string _str_clase;

        public MensajesDat(DALClient objClienteDal, ILogs logs, IOptionsMonitor<ApiSettings> settings)
        {
            _str_clase = GetType().Name;
            _objClienteDal = objClienteDal;
            _logsService = logs;
            _settings = settings.CurrentValue;
        }

        public async Task<RespuestaTransaccion> GetSmsPorProcesar()
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();

            try
            {
                DatosSolicitud ds = new();

                ds.NombreSP = "get_sms_por_procesar";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                respuesta.codigo = "0".ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            }
            catch (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.ToString() );
                await _logsService.SaveExcepcionDataBaseSybase( respuesta, MethodBase.GetCurrentMethod()!.Name, ex, _str_clase );
            }
            return respuesta;
        }

        public async Task<RespuestaTransaccion> ValidarSms(ReqValidarSms reqValidarSms, string str_login, string str_ip_dispositivo)
        {
            RespuestaTransaccion respuesta = new();

            try
            {
                DatosSolicitud ds = new();

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_sms_id", TipoDato = TipoDato.Integer, ObjValue = reqValidarSms.int_sms_id.ToString() } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_error", TipoDato = TipoDato.VarChar } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_error_cod", TipoDato = TipoDato.Integer } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.VarChar, ObjValue = str_login } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_terminal", TipoDato = TipoDato.VarChar, ObjValue = str_ip_dispositivo } );
                
                ds.NombreSP = "set_validar_transaccion_sms";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@str_error" )!.ObjValue.Trim();
                var int_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_error_cod" )!.ObjValue;

                respuesta.codigo = int_codigo!.ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.diccionario.Add( "str_error", str_codigo.ToString() );
            }
            catch (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.ToString() );
                await _logsService.SaveExcepcionDataBaseSybase( respuesta, MethodBase.GetCurrentMethod()!.Name, ex, _str_clase );
            }
            return respuesta;
        }

        public async Task<RespuestaTransaccion> ProcesarTransferencia(ReqProcesarTransf req_procesar_transf, string str_ip_dispositivo)
        {
            RespuestaTransaccion respuesta = new();

            try
            {
                DatosSolicitud ds = new();

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_num_telefono", TipoDato = TipoDato.VarChar, ObjValue = req_procesar_transf.str_num_telefono } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_fecha_transaccion", TipoDato = TipoDato.VarChar, ObjValue = req_procesar_transf.str_fecha_transaccion } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_sms_id", TipoDato = TipoDato.Integer, ObjValue = req_procesar_transf.int_sms_id.ToString() } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ip_disp", TipoDato = TipoDato.VarChar, ObjValue = str_ip_dispositivo } );

                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_error", TipoDato = TipoDato.VarChar } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_error_cod", TipoDato = TipoDato.Integer } );

                ds.NombreSP = "set_procesar_transferencias";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@str_error" )!.ObjValue.Trim();
                var int_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_error_cod" )!.ObjValue;

                respuesta.codigo = int_codigo!.ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.diccionario.Add( "str_error", str_codigo.ToString() );
            }
            catch (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.ToString() );
                await _logsService.SaveExcepcionDataBaseSybase( respuesta, MethodBase.GetCurrentMethod()!.Name, ex, _str_clase );
            }
            return respuesta;
        }

      
        public async Task<RespuestaTransaccion> GuardarSmsSocio(Sms sms, string str_sms_estado)
        {
            RespuestaTransaccion respuesta = new();

            try
            {
                DatosSolicitud ds = new();

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_codigo", TipoDato = TipoDato.Integer, ObjValue = sms.obj_mensaje.int_codigo.ToString() } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_codigo_raiz", TipoDato = TipoDato.VarChar, ObjValue = sms.str_codigo_sms_raiz } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_texto_sms", TipoDato = TipoDato.VarChar, ObjValue = sms.obj_mensaje.str_texto } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_telefono", TipoDato = TipoDato.VarChar, ObjValue = sms.obj_mensaje.str_telefono } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_fecha_recepcion", TipoDato = TipoDato.VarChar, ObjValue = sms.obj_mensaje.str_srv_hora_rec } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_observacion", TipoDato = TipoDato.VarChar, ObjValue = sms.obj_mensaje.str_observacion } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_operadora", TipoDato = TipoDato.VarChar, ObjValue = sms.str_operadora } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_short_code", TipoDato = TipoDato.VarChar, ObjValue = sms.str_short_Code } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_emisor", TipoDato = TipoDato.VarChar, ObjValue = sms.str_emisor } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_estado_sms_proveedor", TipoDato = TipoDato.VarChar, ObjValue = sms.obj_mensaje.str_estado } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_estado_proceso_sms", TipoDato = TipoDato.VarChar, ObjValue = str_sms_estado } );

                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_duplicado", TipoDato = TipoDato.Integer } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_sms_id", TipoDato = TipoDato.Integer } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_error_cod", TipoDato = TipoDato.Integer } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_error", TipoDato = TipoDato.VarChar } );

                ds.NombreSP = "add_sms_socio";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                var int_duplicado = lst_valores.Find( x => x.StrNameParameter == "@int_duplicado" )!.ObjValue;
                var int_sms_id = lst_valores.Find( x => x.StrNameParameter == "@int_sms_id" )!.ObjValue;
                var int_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_error_cod" )!.ObjValue;
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@str_error" )!.ObjValue.Trim();

                respuesta.codigo = int_codigo!.ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.diccionario.Add( "str_error", str_codigo.ToString() );
                respuesta.diccionario.Add( "int_sms_id", int_sms_id );
                respuesta.diccionario.Add( "int_duplicado", int_duplicado );
            }
            catch (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.ToString() );
                await _logsService.SaveExcepcionDataBaseSybase( respuesta, MethodBase.GetCurrentMethod()!.Name, ex, _str_clase );
            }
            return respuesta;
        }

        public async Task<RespuestaTransaccion> ActualizarEstadoProcesoSms(int int_sms_id, string str_estado_sms, string str_login, string str_ip_dispositivo)
        {
            RespuestaTransaccion respuesta = new();

            try
            {
                DatosSolicitud ds = new();

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_sms_id", TipoDato = TipoDato.Integer, ObjValue = int_sms_id.ToString() } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_estado", TipoDato = TipoDato.VarChar, ObjValue = str_estado_sms } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_error", TipoDato = TipoDato.VarChar } );
                ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_error_cod", TipoDato = TipoDato.Integer } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_usuario", TipoDato = TipoDato.VarChar, ObjValue = str_login } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_terminal", TipoDato = TipoDato.VarChar, ObjValue = str_ip_dispositivo } );

                ds.NombreSP = "set_estado_proceso_sms";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@str_error" )!.ObjValue.Trim();
                var int_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_error_cod" )!.ObjValue;

                respuesta.codigo = int_codigo!.ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.diccionario.Add( "str_error", str_codigo.ToString() );
            }
            catch (Exception ex)
            {
                respuesta.codigo = "003";
                respuesta.diccionario.Add( "str_error", ex.ToString() );
                await _logsService.SaveExcepcionDataBaseSybase( respuesta, MethodBase.GetCurrentMethod()!.Name, ex, _str_clase );
            }
            return respuesta;
        }
    }
}
