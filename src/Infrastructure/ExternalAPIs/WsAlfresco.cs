

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Services.Alfresco;
using Domain.Types;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.ExternalAPIs;


public class WsAlfresco : IWsAlfresco
{
    private readonly IHttpService _httpService;
    private readonly ILogs _logService;
    private readonly ApiSettings _settings;
    private readonly string str_clase;
    public WsAlfresco(IHttpService httpService, ILogs logs, IOptionsMonitor<ApiSettings> option)
    {
        _httpService = httpService;
        _logService = logs;
        _settings = option.CurrentValue;
        str_clase = GetType().Name;
    }


    public ResAddDocAlfresco AddDocumentoAlfresco(DocumentoAlfresco documentoAlfresco, string str_operacion, string str_id_transaccion)
    {
        try
        {
            string endpointAlfresco = documentoAlfresco.str_documento_id.Equals( "-1" ) ? "add_documento" : "set_documento_version";
            string str_data = JsonSerializer.Serialize( documentoAlfresco );

            return _httpService.PostRestServiceDataAsync<ResAddDocAlfresco>
                                                            ( str_data,
                                                              _settings.servicio_ws_alfresco + endpointAlfresco,
                                                              String.Empty,
                                                              String.Empty,
                                                              AuthorizationType.BASIC,
                                                              str_id_transaccion
                                                             ).Result;
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = documentoAlfresco, str_operacion = str_operacion };
            _logService.SaveHttpErrorLogs( data, MethodBase.GetCurrentMethod()!.Name, str_clase, ex, str_id_transaccion );
            throw new ArgumentException( str_id_transaccion );
        }

    }

    public ResGetDocAlfresco GetDocumentoAlfresco(string idDocumentoAlfresco, string str_operacion, string str_id_transaccion)
    {
        try
        {
            var data = new { str_id_documento = idDocumentoAlfresco };
            string str_data = JsonSerializer.Serialize( data );

            return _httpService.PostRestServiceDataAsync<ResGetDocAlfresco>
                                                            ( str_data,
                                                              _settings.servicio_ws_alfresco + "get_documento",
                                                              String.Empty,
                                                              String.Empty,
                                                              AuthorizationType.BASIC,
                                                              str_id_transaccion
                                                             ).Result;
        }
        catch (Exception ex)
        {
            var data = new { cuerpo = idDocumentoAlfresco, str_operacion = str_operacion };
            _logService.SaveHttpErrorLogs( data, MethodBase.GetCurrentMethod()!.Name, str_clase, ex, str_id_transaccion );
            throw new ArgumentException( str_id_transaccion );
        }

    }


}