using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SmsSocio.ValidarCodigoSms
{
    public class ReqValidarCodigoSms : Header, IRequest<ResValidarCodigoSms>
    {
        public int int_codigo { get; set; }
    }
}
