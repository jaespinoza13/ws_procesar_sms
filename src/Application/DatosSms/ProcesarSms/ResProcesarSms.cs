using Application.Common.Models;
using Application.Common.ISO20022.Models;

namespace Application.DatosSms.ProcesarSms
{
    public class ResProcesarSms : ResComun
    {
        public List<SmsProcesado> sms_procesados { get; set; }
    }
}
