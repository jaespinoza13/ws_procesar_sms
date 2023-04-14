using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Parametros
{
    public class ResParametro : ResComun
    {
        public List<ParametroSistema> parametros { get; set; }
    }
}
