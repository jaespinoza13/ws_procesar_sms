namespace Application.Common.Models
{
    public class SmsProcesar
    {
        public int int_sms_id { get; set; }
        public string str_sms_texto { get; set; } = String.Empty;
        public string str_telefono { get; set; } = String.Empty;
        public string str_fecha_recepcion { get; set; } = String.Empty;
        public string str_fecha_registro { get; set; } = String.Empty;
    }
}
