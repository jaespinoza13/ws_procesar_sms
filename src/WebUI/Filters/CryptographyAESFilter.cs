using Application.Common.Converting;
using Application.Common.Cryptography;
using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace WebUI.Filters
{
    public class CryptographyAESFilter : IActionFilter
    {

        private readonly IKeysDat _keysDat;
        private readonly ApiSettings _settings;

        public CryptographyAESFilter(IKeysDat keysDat, IOptionsMonitor<ApiSettings> options)
        {
            _keysDat = keysDat;
            _settings = options.CurrentValue;

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Desencripta datos de entrada
            var modelRequest = context.ActionArguments.First();
            var reqGetKeys = JsonSerializer.Deserialize<ReqGetKeys>( JsonSerializer.Serialize( modelRequest.Value ) )!;

            if (_settings.lst_canales_encriptar.Contains( reqGetKeys.str_nemonico_canal ))
            {
                var res_tran = _keysDat.GetKeys( reqGetKeys );
                var Key = Conversions.ConvertConjuntoDatosToClass<ResGetKeys>( (ConjuntoDatos)res_tran.cuerpo, 0 );

                if (Key != null)
                    try
                    {
                        if (modelRequest.Value!.GetType().GetMethod( "DecryptAES" ) != null)
                            modelRequest.Value!.GetType().GetMethod( "DecryptAES" )!.Invoke( modelRequest.Value, new object[] { Key } );


                        modelRequest.Value.GetType().GetMethod( "DecryptAESHeader" )!.Invoke( modelRequest.Value, new object[] { Key } );


                    }
                    catch (Exception)
                    {
                        GenerarExcepcion( context );
                    }
                else
                    GenerarExcepcion( context );

            }




        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result != null)
            {
                var response = ((ObjectResult)context.Result!).Value;


                if (response != null)
                {
                    var reqGetKeys = JsonSerializer.Deserialize<ReqGetKeys>( JsonSerializer.Serialize( response ) )!;
                    if (_settings.lst_canales_encriptar.Contains( reqGetKeys.str_nemonico_canal ))
                    {
                        var res_tran = _keysDat.GetKeys( reqGetKeys );
                        var Key = Conversions.ConvertConjuntoDatosToClass<ResGetKeys>( (ConjuntoDatos)res_tran.cuerpo, 0 );

                        if (Key != null)
                            try
                            {
                                if (response.GetType().GetMethod( "EncryptAES" ) != null)

                                    response.GetType().GetMethod( "EncryptAES" )!.Invoke( response, new object[] { Key } );


                                response.GetType().GetMethod( "EncryptAESHeader" )!.Invoke( response, new object[] { Key } );


                            }
                            catch (Exception)
                            {
                                throw new ArgumentException( "Error: Credenciales inválidas 002" );
                            }
                        else
                            throw new ArgumentException( "Error: Credenciales inválidas 001" );
                    }
                }
            }
        }

        public void GenerarExcepcion(ActionExecutingContext context)
        {
            ResException resException = new();
            resException.str_res_codigo = Convert.ToInt32( HttpStatusCode.Unauthorized ).ToString();
            resException.str_res_id_servidor = "Error: Credenciales inválidas";
            resException.str_res_estado_transaccion = "ERR";
            resException.dt_res_fecha_msj_crea = DateTime.Now;
            resException.str_res_info_adicional = "Tu sesión ha caducado, por favor ingresa nuevamente.";

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Result = new ObjectResult( resException );
        }
    }
}
