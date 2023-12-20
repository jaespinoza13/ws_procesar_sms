

using System.Data;

namespace Application.Common.Models
{
    public class Notificacion
    {
        public int int_id { get; set; }
        public string str_destinos { get; set; } = String.Empty;
        public string str_num_documento { get; set; } = String.Empty;
        public string str_nombre_servicio { get; set; } = String.Empty;
        public string str_operacion { get; set; } = String.Empty;
        public string str_nemonico_plantilla { get; set; } = String.Empty;
        public int int_sistema { get; set; }
        public int int_tabla { get; set; }
        public string str_plantilla { get; set; } = String.Empty;
        public int int_destino_jefe { get; set; }
        public string str_configuracion { get; set; } = String.Empty;
        public string str_variables { get; set; } = String.Empty;
        public string str_ids_documentos { get; set; } = String.Empty;
        public string[]? lista_variables { get; set; }
        public string[]? lista_valores { get; set; }
        public string[]? lista_destinos { get; set; }
        public string[]? lista_ids_documentos { get; set; }
        public DataSet? dts_detalles { get; set; }
        public string str_error { get; set; } = String.Empty;
        public string str_asunto { get; set; } = String.Empty;
        public string str_html { get; set; } = String.Empty;

    }
}
