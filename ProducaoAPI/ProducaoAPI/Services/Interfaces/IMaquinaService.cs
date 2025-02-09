using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IMaquinaService
    {
        Task<IEnumerable<Maquina>> ListarMaquinasAtivas();
        Task<IEnumerable<Maquina>> ListarTodasMaquinas();
        Task<Maquina> BuscarMaquinaPorIdAsync(int id);
        Task AdicionarAsync(Maquina maquina);
        Task AtualizarAsync(Maquina maquina);
        MaquinaResponse EntityToResponse(Maquina maquina);
        ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas);
        Task ValidarDadosParaCadastrar(MaquinaRequest request);
        Task ValidarDadosParaAtualizar(MaquinaRequest request, int id);

    }
}
