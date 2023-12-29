using Application.Common.Models;
using Application.SmsSocio.ValidarSms;
using Application.SmsSocio.ProcesarTransf;

namespace Application.Interfaz
{
    public interface IMensajesDat
    {
        Task<RespuestaTransaccion> GetSmsPorProcesar();
        Task<RespuestaTransaccion> ProcesarTransferencia(ReqProcesarTransf req_procesar_transf, string str_ip_dispositivo);
        Task<RespuestaTransaccion> ValidarSms(ReqValidarSms reqValidarSms, string str_login, string str_ip_dispositivo);
        Task<RespuestaTransaccion> GuardarSmsSocio(Sms sms, string str_sms_estado);
        Task<RespuestaTransaccion> ActualizarEstadoProcesoSms(int int_sms_id, string str_estado_sms, string str_login, string str_ip_dispositivo);

    }
}


