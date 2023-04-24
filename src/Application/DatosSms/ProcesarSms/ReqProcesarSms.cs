using MediatR;
using Application.Common.Models;
using Application.Common.ISO20022.Models;

namespace Application.DatosSms.ProcesarSms
{
    public class ReqProcesarSms : Header, IRequest<ResProcesarSms>
    {
        public Sms sms { get; set; }
    }
}
