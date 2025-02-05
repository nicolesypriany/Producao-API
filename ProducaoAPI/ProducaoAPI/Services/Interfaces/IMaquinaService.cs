using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IMaquinaService
    {
        Task<IEnumerable<Maquina>> ListarMaquinasAsync();
        Task<Maquina> BuscarMaquinaPorIdAsync(int id);
        Task AdicionarAsync(Maquina maquina);
        Task AtualizarAsync(Maquina maquina);
        MaquinaResponse EntityToResponse(Maquina maquina);
        ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas);
        Task ValidarDados(MaquinaRequest request);

    }
}
