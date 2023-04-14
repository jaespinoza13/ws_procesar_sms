
using Application.Common.ISO20022.Models;

namespace Application.Common.Interfaces;
public interface ISqlInjectionValidationService
{
    bool Validate(string str_palabra_validar, string campo, string str_operacion, Header header, string str_clase);
}