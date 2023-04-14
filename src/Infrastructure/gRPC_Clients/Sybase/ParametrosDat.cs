using AccesoDatosGrpcAse.Neg;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Parametros;
using Grpc.Net.Client;
using Infrastructure.Common.Funciones;
using Infrastructure.MemoryCache;
using Microsoft.Extensions.Options;
using System.Reflection;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infrastructure.gRPC_Clients.Sybase;
internal class ParametrosDat : IParametrosDat
{
    private readonly ApiSettings _settings;
    private readonly DALClient _objClienteDal;
    private readonly ILogs _logsService;
    private readonly string str_clase;

    public ParametrosDat(IOptionsMonitor<ApiSettings> options, ILogs logsService, DALClient objClienteDal)
    {
        _settings = options.CurrentValue;
        _logsService = logsService;

        this.str_clase = GetType().FullName!;

        _objClienteDal = objClienteDal;

    }

    public RespuestaTransaccion GetParametros(dynamic reqGetParametros)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            DatosSolicitud ds = new();
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_id_transaccion", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_id_transaccion } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_id_sistema", TipoDato = TipoDato.Integer, ObjValue = reqGetParametros.str_id_sistema } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_login", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_login } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nemonico_canal", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_nemonico_canal } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_ip_dispositivo", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_ip_dispositivo } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_sesion", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_sesion } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_mac_dispositivo", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_mac_dispositivo } );

            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@str_o_error", TipoDato = TipoDato.VarChar } );
            ds.ListaPSalida.Add( new ParametroSalida { StrNameParameter = "@int_o_error_cod", TipoDato = TipoDato.Integer } );

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nombre", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_nombre.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@str_nemonico", TipoDato = TipoDato.VarChar, ObjValue = reqGetParametros.str_nemonico.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@int_front", TipoDato = TipoDato.Integer, ObjValue = 0.ToString() } );

            ds.NombreSP = "get_parametros";
            ds.NombreBD = _settings.DB_meg_megonline;

            var resultado = _objClienteDal.ExecuteDataSet( ds );
            var lst_valores = new List<ParametroSalidaValores>();

            foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add( item );
            var str_codigo = lst_valores.Find( x => x.StrNameParameter == "@int_o_error_cod" )!.ObjValue;
            var str_error = lst_valores.Find( x => x.StrNameParameter == "@str_o_error" )!.ObjValue.Trim();

            respuesta.codigo = str_codigo.ToString().Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            respuesta.diccionario.Add( "str_error", str_error.ToString() );
        }
        catch (Exception exception)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", exception.ToString() );
            _logsService.SaveExcepcionDataBaseSybase( reqGetParametros, MethodBase.GetCurrentMethod()!.Name, exception, str_clase );
            throw new ArgumentException( reqGetParametros.str_id_transaccion )!;
        }
        return respuesta;
    }

    public RespuestaTransaccion GetAllParametros(ReqParametro reqRapametro)
    {
        var respuesta = new RespuestaTransaccion();

        try
        {
            DatosSolicitud ds = new();

            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@id_sistema", TipoDato = TipoDato.Integer, ObjValue = reqRapametro.id_sistema.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@valor_busqueda", TipoDato = TipoDato.VarChar, ObjValue = reqRapametro.valor_busqueda } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@campo_busqueda", TipoDato = TipoDato.VarChar, ObjValue = reqRapametro.campo_busqueda } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@vigencia", TipoDato = TipoDato.Integer, ObjValue = reqRapametro.vigencia.ToString() } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@fecha_desde", TipoDato = TipoDato.VarChar, ObjValue = reqRapametro.fecha_desde } );
            ds.ListaPEntrada.Add( new ParametroEntrada { StrNameParameter = "@fecha_hasta", TipoDato = TipoDato.VarChar, ObjValue = reqRapametro.fecha_hasta } );

            ds.NombreSP = "get_parametros";
            ds.NombreBD = _settings.DB_meg_sistemas;

            var resultado = _objClienteDal.ExecuteDataSet( ds );
            var lst_valores = new List<ParametroSalidaValores>();

            respuesta.codigo = "0".ToString().Trim().PadLeft( 3, '0' );
            respuesta.cuerpo = Funciones.ObtenerDatos( resultado );
            //respuesta.diccionario.Add( "str_error", str_error.ToString() );
        }
        catch (Exception exception)
        {
            respuesta.codigo = "003";
            respuesta.diccionario.Add( "str_error", exception.ToString() );
            _logsService.SaveExcepcionDataBaseSybase( reqRapametro, MethodBase.GetCurrentMethod()!.Name, exception, str_clase );
            throw new ArgumentException( reqRapametro.id_sistema.ToString() )!;
        }
        return respuesta;
    }
}

