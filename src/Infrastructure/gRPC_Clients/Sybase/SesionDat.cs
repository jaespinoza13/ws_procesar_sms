using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;

using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Microsoft.Extensions.Options;
using System.Reflection;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.gRPC_Clients.Sybase
{
    public interface ISesionDat
    {
        RespuestaTransaccion ControlSesion(ValidaSesion validaSesion);
    }

    internal class SesionDat : ISesionDat
    {
        private readonly ApiSettings _settings;
        private readonly DALClient _objClienteDal;
        private readonly ILogs _logsService;
        private readonly string str_clase;

        public SesionDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
        {
            _settings = options.CurrentValue;
            _logsService = logsService;
            _objClienteDal = objClienteDal;
            this.str_clase = GetType().FullName!;
        }


        public RespuestaTransaccion ControlSesion(ValidaSesion validaSesion)
        {
            var respuesta = new RespuestaTransaccion();

            try
            {
                DatosSolicitud ds = new DatosSolicitud();
                Funciones.llenar_datos_auditoria_salida( ds, validaSesion );

                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_login", TipoDato = TipoDato.Integer, ObjValue = validaSesion.str_id_usuario.ToString() } );
                ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_estado", TipoDato = TipoDato.Integer, ObjValue = validaSesion.int_estado.ToString() } );


                ds.NombreSP = "get_validar_estado_sesion";
                ds.NombreBD = _settings.DB_meg_servicios;

                var resultado = _objClienteDal.ExecuteDataSet( ds );
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
                var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
                var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

                respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
                respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
                respuesta.diccionario.Add( "str_error", str_error );

            }
            catch (Exception exception)
            {
                respuesta.codigo = "001";
                respuesta.diccionario.Add( "str_error", exception.ToString() );
                _logsService.SaveExcepcionDataBaseSybase( validaSesion, MethodBase.GetCurrentMethod()!.Name, exception, str_clase );
                throw new ArgumentException( validaSesion.str_id_transaccion );
            }
            return respuesta;
        }
    }
}
