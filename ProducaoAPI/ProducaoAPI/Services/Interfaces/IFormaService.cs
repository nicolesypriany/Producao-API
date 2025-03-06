using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IFormaService
    {
        Task<IEnumerable<Forma>> ListarFormasAtivas();
        Task<IEnumerable<Forma>> ListarTodasFormas();
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task<Forma> AdicionarAsync(FormaRequest request);
        Task<Forma> AtualizarAsync(int id, FormaRequest request);
        Task<Forma> InativarForma(int id);
        FormaResponse EntityToResponse(Forma forma);
        ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma);
        Task<List<Maquina>> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas);
    }
}
