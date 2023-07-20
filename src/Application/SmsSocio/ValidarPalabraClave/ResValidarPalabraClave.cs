using Domain.Entities;
using Application.Common.ISO20022.Models;

namespace Application.SmsSocio.ValidarPalabraClave
{
    public class ResValidarPalabraClave : ResComun
    {
        public List<PalabrasSms> palabras_clave { get; set; } = new List<PalabrasSms>();
    }
}
