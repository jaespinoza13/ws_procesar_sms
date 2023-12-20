



using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.gRPC_Clients.Sybase;

namespace Infrastructure.SessionControl;

public class SessionControl : ISessionControl
{
    private readonly ISesionDat _sesionDat;
    public SessionControl(ISesionDat sessionDat) => _sesionDat = sessionDat;

    public RespuestaTransaccion SessionControlFilter(ValidaSesion bodyRequest)
    {
        return  _sesionDat.ControlSesion( bodyRequest );
    }
}

