using Application.Common.Converting;
using Application.Common.Interfaces;
using Application.Common.ISO20022.Models;
using Application.Common.Models;

using Domain.Parameters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.MemoryCache;

public class GetParameters : Header
{
    public string str_nombre { get; set; } = "";
    public string str_nemonico { get; set; } = "-1";
    public int int_front { get; set; }
}

internal class ParametersInMemory : IParametersInMemory
{
    public readonly ApiSettings _settings;
    public readonly IMemoryCache _memoryCache;
    public DateTime dt_fecha_codigos { get; set; }

    public ParametersInMemory(IOptionsMonitor<ApiSettings> options, /*IParametrosDat parametros,*/ IMemoryCache memoryCache)
    {
        this._settings = options.CurrentValue;
        this._memoryCache = memoryCache;
    }


    public Parametro FindErrorCode(string str_codigo)
    {
        var listadoErrores = _memoryCache.Get<List<Parametro>>( "Errores" );
        return listadoErrores.Find( x => x.str_valor_ini == str_codigo )!;
    }
    public Parametro FindParameter(string str_nemonico)
    {
        var listadoParametros = _memoryCache.Get<List<Parametro>>( "Parametros" );
        return listadoParametros.Find( x => x.str_nemonico == str_nemonico )!;
    }

    public List<Parametro> FindParameterName(string str_nombre)
    {
        var lst_parametros = _memoryCache.Get<List<Parametro>>( "Parametros" );
        return lst_parametros.FindAll( x => x.str_nombre == str_nombre );
    }

    public List<Parametro> lst_errores() => _memoryCache.Get<List<Parametro>>( "Errores" );
    public List<Parametro> lst_parametros() => _memoryCache.Get<List<Parametro>>( "Parametros" );
}
