namespace Application.SmsSocio.ValidarSms
{
    public class ReqValidarSms
    {
        public int int_codigo { get; set; }
        public string str_codigo_raiz { get; set; } = String.Empty;
        public string str_texto_sms { get; set; } = String.Empty;
        public string str_telefono { get; set; } = String.Empty;
        public string str_fecha_recepcion { get; set; } = String.Empty;
        public string str_observacion { get; set; } = String.Empty;
        public string str_operadora { get; set; } = String.Empty;
        public string str_short_code { get; set; } = String.Empty;
        public string str_emisor { get; set; } = String.Empty;
        public string str_estado_sms_proveedor { get; set; } = String.Empty;
    }
}
