

using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface ISessionControl
{
    RespuestaTransaccion SessionControlFilter(ValidaSesion bodyRequest);
}