
using Domain.Services.Alfresco;

namespace Application.Common.Interfaces;

public interface IWsAlfresco
{
    ResAddDocAlfresco AddDocumentoAlfresco(DocumentoAlfresco documentoAlfresco, string str_operacion, string str_id_transaccion);
    ResGetDocAlfresco GetDocumentoAlfresco(string idDocumentoAlfresco, string str_operacion, string str_id_transaccion);
}