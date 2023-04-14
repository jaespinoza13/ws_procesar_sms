using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CamposParametros
    {
        public int idParametro { get; set; }
        public string nemonico { get; set; } = String.Empty;
        public string descripcion { get; set; } = String.Empty;
    }
}
