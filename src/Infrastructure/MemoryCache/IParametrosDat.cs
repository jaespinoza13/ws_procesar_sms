using Application.Common.Models;

namespace Infrastructure.MemoryCache
{
    public interface IParametrosDat
    {
        RespuestaTransaccion GetParametros(dynamic reqGetParametros);
    }
}
