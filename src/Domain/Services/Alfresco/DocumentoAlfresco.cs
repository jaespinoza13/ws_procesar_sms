
namespace Domain.Services.Alfresco
{
    public class DocumentoAlfresco
    {
        public int int_ente { get; set; }
        public int int_tipo_identifica { get; set; }
        public string str_num_identifica { get; set; } = String.Empty;
        public string str_documento_id { get; set; } = String.Empty;
        public string str_referencia { get; set; } = String.Empty;
        public string str_path_repositorio { get; set; } = String.Empty;
        public string str_modelo_doc_alfresco { get; set; } = String.Empty;
        public string str_nombre { get; set; } = String.Empty;
        public string str_observacion { get; set; } = String.Empty;
        public string str_doc_extencion { get; set; } = String.Empty;
        public string file_bytes { get; set; } = String.Empty;
        public int int_sistema { get; set; }
        public int int_oficina { get; set; }
        public int int_perfil { get; set; }
        public string str_usuario { get; set; } = String.Empty;
        public string str_terminal { get; set; } = String.Empty;
    }
}
