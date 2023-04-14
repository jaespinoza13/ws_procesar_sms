using Application.Common.ISO20022.Models;
using Domain.Entities;

namespace Application.SmsSocio.ValidarPalabraClave
{
    public class ResValidarPalabraClave : ResComun
    {
        public List<PalabrasSms> palabras_clave { get; set; }
    }
}
