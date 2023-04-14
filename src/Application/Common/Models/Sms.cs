using Domain.Entities;

namespace Application.Common.Models
{
    public class Sms
    {
        public string str_emisor { get; set; }
        public string str_short_Code { get; set; }
        public string str_operadora { get; set; }
        public string str_codigo_sms_raiz { get; set; }
        public Mensaje obj_mensaje { get; set; }
    }
}
