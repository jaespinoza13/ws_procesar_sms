﻿namespace Application.Parametros
{
    public class ReqParametro
    {
        public int id_sistema { get; set; }
        public string valor_busqueda { get; set; } = string.Empty;
        public string campo_busqueda { get; set; } = string.Empty;
        public int vigencia { get; set; }
        public string fecha_desde { get; set; } = string.Empty;
        public string fecha_hasta { get; set; } = string.Empty;
    }
}
