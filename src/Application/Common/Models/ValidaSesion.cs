

using Application.Common.ISO20022.Models;

namespace Application.Common.Models
{
    public class ValidaSesion : Header
    {
        public int int_id_login { get; set; }
        public int int_estado { get; set; }
        public int int_estado_login { get; set; }
    }
}
