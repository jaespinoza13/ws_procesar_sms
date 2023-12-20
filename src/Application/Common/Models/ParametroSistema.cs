namespace Application.Common.Models
{
    public class ParametroSistema
    {
        public int idParametro { get; set; }
        public int idSistema { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string nemonico { get; set; } = string.Empty;
        public string valorIni { get; set; } = string.Empty;
        public string valorFin { get; set; } = string.Empty;
        public string fechaDesde { get; set; } = string.Empty;
        public string fechaHasta { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string vigencia { get; set; } = string.Empty;
    }
}
