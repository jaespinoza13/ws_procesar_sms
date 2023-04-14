using Application.Common.ISO20022.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SmsSocio.ValidarSms
{
    public class ResValidarSms : ResComun
    {
        public List<PalabrasSms> palabras_clave { get; set; }
    }
}
