using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Services.Interfaces
{
    public interface ILogServices
    {
        Task CriarLogAdicionar(Type tipoObjeto, int objetoId);
        Task CriarLogAtualizar(Type tipoObjeto, Type tipoRequest, Object objeto, Object request, int objetoId);
        Task CriarLogInativar(Type tipoObjeto, int objetoId);
        Task<IEnumerable<Log>> BuscarLogs(LogRequest request);
    }
}