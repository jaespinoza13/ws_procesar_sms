
using Application.Common.ISO20022.Models;
using Application.Common.Models;

namespace Application.Common.Functions;

internal static class Functions
{
    public static Cabecera LlenarCabeceraSolicitud(Header header)
    {
        Cabecera cabecera = new();
        cabecera.int_sistema = Convert.ToInt32( header.str_id_sistema );
        cabecera.str_usuario = header.str_login;
        cabecera.int_perfil = Convert.ToInt32( header.str_id_oficina );
        cabecera.int_oficina = Convert.ToInt32( header.str_id_oficina );
        cabecera.str_nemonico_canal = header.str_nemonico_canal;
        cabecera.str_ip = header.str_ip_dispositivo;
        cabecera.str_session = header.str_sesion;
        cabecera.str_mac = header.str_mac_dispositivo;
        cabecera.str_nombre_canal = header.str_app;

        return cabecera;
    }
}
