using MediatR;
using Application.Common.Models;
using Application.Common.ISO20022.Models;

namespace Application.DatosSms.ObtenerSms
{
    public class ReqObtenerSms : Header, IRequest<ResObtenerSms>
    {
        public Sms sms { get; set; } = new Sms();
    }
}
