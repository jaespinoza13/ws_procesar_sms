
namespace Application.Common.Interfaces;

public interface IHttpService
{
    Task<T> GetRestServiceDataAsync<T>(string serviceAddress);
    Task<T> PostRestServiceDataAsync<T>(string serializedData,
                                        string serviceAddress,
                                        string parameters,
                                        string auth,
                                        string authorizationType,
                                        string str_id_transaccion);
}