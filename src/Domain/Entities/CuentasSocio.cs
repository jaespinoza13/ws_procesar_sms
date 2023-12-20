using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CuentasSocio
    {
        public int int_cuenta { get; set; }
        public string str_numero_cuenta { get; set; } = String.Empty;
        public string str_nombre { get; set; } = String.Empty;
    }
}
