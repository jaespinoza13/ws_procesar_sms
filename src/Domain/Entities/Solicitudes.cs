using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Solicitudes
    {
        public int int_id { get; set; }
        public int int_ente { get; set; }
        public string str_cedula { get; set; } = String.Empty;
        public string str_nombre { get; set; } = String.Empty;
        public string str_num_cuenta { get; set; } = String.Empty;
        public string str_nombre_orden_pago { get; set; } = String.Empty;
        public string str_tipo_libretin { get; set; } = String.Empty;
        public int int_estado { get; set; }
        public string str_estado { get; set; } = String.Empty;
        public string str_cedula_retira { get; set; } = String.Empty;
        public string str_nombre_retira { get; set; } = String.Empty;
        public int int_oficina_crea { get; set; }
        public string str_oficina_crea { get; set; } = String.Empty;
        public int int_oficina_retira { get; set; }
        public string str_oficina_retira { get; set; } = String.Empty;
        public string str_nombre_tipo_libretin { get; set; } = String.Empty;
        public string str_fecha_crea { get; set; } = String.Empty;
        public string str_usuario_crea { get; set; } = String.Empty;
        public string str_usuario_aprueba { get; set; } = String.Empty;
        public string str_fecha_aprueba { get; set; } = String.Empty;
        public bool bit_aprobar { get; set; }
        public bool bit_entregar { get; set; }
        public bool bit_imprimir { get; set; }
        public bool bit_negar { get; set; }
        public bool bit_anular { get; set; }
        public bool bit_cargar_doc_sol { get; set; }
        public bool bit_descargar_doc_sol { get; set; }
        public bool bit_cargar_doc_entrega { get; set; }
        public bool bit_descargar_doc_entrega { get; set; }
        public int int_numero_libretin { get; set; }
        public string str_codigo_doc_sol { get; set; } = String.Empty;
        public string str_codigo_doc_acta { get; set; } = String.Empty;
        public string str_codigo_estado { get; set; } = String.Empty;
        public int int_orden { get; set; }
        public bool bit_workflow_log { get; set; }
        public bool bit_activar { get; set; }
    }
}
