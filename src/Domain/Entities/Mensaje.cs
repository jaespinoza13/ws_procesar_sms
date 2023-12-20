namespace Domain.Entities
{
    public class Mensaje
    {
        public int int_codigo { get; set; }
        public string str_telefono { get; set; } = String.Empty;
        public string str_texto { get; set; } = String.Empty;
        public string str_srv_fecha_rec { get; set; } = String.Empty;
        public string str_srv_hora_rec { get; set; } = String.Empty;
        public string str_observacion { get; set; } = String.Empty;
        public string str_estado { get; set; } = String.Empty;
    }
}
