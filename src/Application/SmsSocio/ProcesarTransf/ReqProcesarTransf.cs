namespace Application.SmsSocio.ProcesarTransf
{
    public class ReqProcesarTransf
    {
        public string str_num_telefono { set; get; } = String.Empty;
        public string str_fecha_transaccion { set; get; } = String.Empty;
        public int int_sms_id { set; get; }
    }
}
