using Application.Common.ISO20022.Models;

namespace Application.DatosSms.ProcesarSms
{
    public class ResProcesarSmsLogs : ResComun
    {
        public string codigo { get; set; } = String.Empty;
        public string mensaje { get; set; } = String.Empty;
    }
}
