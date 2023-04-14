

namespace Application.Common.Models
{
    public class SolicitudTransaccion
    {
        public Cabecera cabecera { get; set; }
        public Object cuerpo { get; set; }
        public SolicitudTransaccion(Cabecera cabecera, Object cuerpo)
        {
            this.cabecera = cabecera;
            this.cuerpo = cuerpo;
        }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public SolicitudTransaccion()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {

        }
    }
}
