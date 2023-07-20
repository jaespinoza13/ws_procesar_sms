using Domain.Entities;

namespace Application.Common.Models
{
    public class Sms
    {
        public string str_emisor { get; set; } = String.Empty;
        public string str_short_Code { get; set; } = String.Empty;
        public string str_operadora { get; set; } = String.Empty;
        public string str_codigo_sms_raiz { get; set; } = String.Empty;
        public Mensaje obj_mensaje { get; set; } = new Mensaje();
    }
}
