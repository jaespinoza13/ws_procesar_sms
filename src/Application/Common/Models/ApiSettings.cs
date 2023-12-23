namespace Application.Common.Models
{
    public class ApiSettings
    {
        public string? DB_meg_megonline { get; set; }
        public string? DB_meg_servicios { get; set; }
        public string? DB_meg_sistemas { get; set; }
        public string? DB_meg_bce { get; set; }


        public string? client_grpc_sybase { get; set; }
        public string? client_grpc_mongo { get; set; }
        public int timeoutGrpcSybase { get; set; } = 10;
        public int delayOutGrpcSybase { get; set; } = 10;
        public int timeoutGrpcMongo { get; set; } = 5;
        public int delayOutGrpcMongo { get; set; } = 5;


        public string nombre_base_mongo { get; set; } = String.Empty;
        public string coll_peticiones { get; set; } = String.Empty;
        public string coll_errores { get; set; } = String.Empty;
        public string coll_amenazas { get; set; } = String.Empty;
        public string coll_respuesta { get; set; } = String.Empty;
        public string coll_peticiones_diarias { get; set; } = String.Empty;
        public string coll_promedio_peticiones_diarias { get; set; } = String.Empty;
        public string coll_errores_db { get; set; } = String.Empty;
        public string coll_errores_http { get; set; } = String.Empty;


        public string logs_path_peticiones { get; set; } = String.Empty;
        public string logs_path_errores { get; set; } = String.Empty;
        public string logs_path_errores_db { get; set; } = String.Empty;
        public string logs_path_amenazas { get; set; } = String.Empty;
        public string logs_path_errores_http { get; set; } = String.Empty;


        public List<int> lst_codigos_error_sistemas { get; set; } = new();
        public List<string> lst_nombres_parametros { get; set; } = new();
        public List<string> lst_canales_encriptar { get; set; } = new();

        public bool valida_peticiones_diarias { get; set; }
        public int timeOutHttp { get; set; }

        public string auth_ws_procesar_sms_user { get; set; } = String.Empty;

        public string auth_ws_procesar_sms_pass { get; set; } = String.Empty;
                
        public int mostrar_descripcion_badrequest { get; set; }

        public string path_logo_png { get; set; } = String.Empty;

        

    }
}