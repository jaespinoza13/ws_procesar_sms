using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Interfaz;
using System.Reflection;

namespace Application.Parametros
{
    public class SisParametros
    {
        public readonly ILogs _logsService;
        private readonly string _clase;
        private readonly IParametroDat _parametro;

        public SisParametros(ILogs logsService, IParametroDat parametro)
        {
            _logsService = logsService;
            _clase = GetType().Name;
            _parametro = parametro;
        }

        public async Task<ResParametro> GetParametro(ReqParametro request)
        {
            string strOperacion = "OBTENER_PARAMETROS";
            ResParametro respuesta = new();
            try
            {
                var res_tran = await _parametro.GetAllParametros( request );
                respuesta.str_res_estado_transaccion = res_tran.codigo.Equals( "000" ) ? "OK" : "ERR";
                respuesta.str_res_codigo = res_tran.codigo;

                respuesta.parametros = Conversions.ConvertConjuntoDatosToListClass<ParametroSistema>( (ConjuntoDatos)res_tran.cuerpo );
                await _logsService.SaveResponseLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase );
                return respuesta;
            }
            catch (Exception ex)
            {
                await _logsService.SaveExceptionLogs( respuesta, strOperacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex );
                throw new ArgumentException("Error al obtener los parametros.");
            }
        }
    }
}
