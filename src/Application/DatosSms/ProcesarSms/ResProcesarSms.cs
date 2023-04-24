namespace Application.DatosSms.ProcesarSms
{
    public class ResProcesarSms
    {
        public DateTime dt_fecha_operacion { get; set; } = DateTime.Now;
        public string str_id_transaccion { get; set; } = String.Empty;
        public string codigo { get; set; } = String.Empty;
        public string mensaje { get; set; } = String.Empty;
    }
}
