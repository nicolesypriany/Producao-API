using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IFormaService
    {
        Task<IEnumerable<Forma>> ListarFormasAsync();
        Task<Forma> BuscarFormaPorIdAsync(int id);
        Task AdicionarAsync(Forma forma);
        Task AtualizarAsync(Forma forma);
        FormaResponse EntityToResponse(Forma forma);
        ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma);
        List<Maquina> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas);
    }
}
