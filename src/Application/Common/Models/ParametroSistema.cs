namespace Application.Common.Models
{
    public class ParametroSistema
    {
        public int idParametro { get; set; }
        public int idSistema { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string valorIni { get; set; }
        public string valorFin { get; set; }
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string descripcion { get; set; }
        public string vigencia { get; set; }
    }
}
