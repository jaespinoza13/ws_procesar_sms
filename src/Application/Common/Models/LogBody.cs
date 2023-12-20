namespace Application.Common.Models
{
    public class LogBody
    {
        public string str_id_transaccion { get; set; } = String.Empty;
        public string str_operacion { get; set; } = String.Empty;
        public DateTime dt_fecha_operacion { get; set; } = DateTime.ParseExact( DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss" ), "yyyy-MM-dd HH:mm:ss", null );
    }
}
