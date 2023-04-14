using Application.Common.ISO20022.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SmsSocio.ValidarSms
{
    public class ReqValidarSms : Header, IRequest<ResValidarSms>
    {
        public int int_codigo { get; set; }
        public string str_codigo_raiz { get; set; } = String.Empty;
        public string str_texto_sms { get; set; }
        public string str_telefono { get; set; }
        public string str_fecha_recepcion { get; set; }
        public string str_observacion { get; set; } = String.Empty;
        public string str_operadora { get; set; }
        public string str_short_code { get; set; }
        public string str_emisor { get; set; }
        public string str_estado_sms_proveedor { get; set; }
    }
}
