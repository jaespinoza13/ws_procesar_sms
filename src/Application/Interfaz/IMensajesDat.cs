using Application.Common.Models;
using Application.SmsSocio.ValidarSms;

namespace Application.Interfaz
{
    public interface IMensajesDat
    {
        Task<RespuestaTransaccion> ValidarCodigoSms(int int_codigo_sms);
        Task<RespuestaTransaccion> ValidarPalabraClaveBloquear(string str_texto_sms);
        Task<RespuestaTransaccion> ProcesarTransferencia(string str_num_telefono, string str_fecha_transaccion, int int_sms_id);
        Task<RespuestaTransaccion> ValidarSms(ReqValidarSms reqValidarSms);
        Task<RespuestaTransaccion> GuardarSmsSocio(Sms sms, string str_sms_estado);
        Task<RespuestaTransaccion> ActualizarEstadoProcesoSms(int int_sms_id, string str_estado_sms);

    }
}


