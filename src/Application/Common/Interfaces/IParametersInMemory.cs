
using Domain.Parameters;

namespace Application.Common.Interfaces;
public interface IParametersInMemory
{
    List<Parametro> lst_errores();
    List<Parametro> lst_parametros();
    void ValidateCachedParameters();
    void LoadParameters();
    Parametro FindErrorCode(string str_codigo);
    Parametro FindParameter(string str_nemonico);
    List<Parametro> FindParameterName(string str_nombre);
}