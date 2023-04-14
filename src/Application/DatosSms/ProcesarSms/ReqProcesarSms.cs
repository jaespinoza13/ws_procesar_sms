using Application.Common.ISO20022.Models;
using Application.Common.Models;
using MediatR;

namespace Application.DatosSms.ProcesarSms
{
    public class ReqProcesarSms : Header, IRequest<ResProcesarSms>
    {
        public Sms sms { get; set; }
    }
}
