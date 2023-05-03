using Application.Common.Models;

namespace Application.SmsSocio.AgregarSms
{
    public class ReqAgregarSms
    {
        public Sms sms { get; set; }
        public string str_sms_estado { get; set; } = string.Empty;
    }
}
