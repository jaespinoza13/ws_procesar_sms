using Application.Common.Models;
using Application.Parametros;

namespace Application.Interfaz
{
    public interface IParametroDat
    {
        Task<RespuestaTransaccion> GetAllParametros(ReqParametro reqRapametro);
    }
}
