using Domain.Entities;

namespace Application.SmsSocio.ValidarSms
{
    public class ResValidarSms
    {
        public string str_res_estado_transaccion { get; set; } = String.Empty;
        public string str_res_codigo { get; set; } = String.Empty;
        public string str_res_info_adicional { get; set; } = String.Empty;
        public List<PalabrasSms> palabras_clave { get; set; } = new List<PalabrasSms>();
    }
}
