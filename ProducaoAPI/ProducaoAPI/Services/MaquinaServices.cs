using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class MaquinaServices : IMaquinaService
    {
        private readonly IMaquinaRepository _maquinaRepository;
        public MaquinaServices(IMaquinaRepository maquinaRepository)
        {
            _maquinaRepository = maquinaRepository;
        }
        public MaquinaResponse EntityToResponse(Maquina maquina)
        {
            return new MaquinaResponse(maquina.Id, maquina.Nome, maquina.Marca, maquina.Ativo);
        }
        public ICollection<MaquinaResponse> EntityListToResponseList(IEnumerable<Maquina> maquinas)
        {
            return maquinas.Select(m => EntityToResponse(m)).ToList();
        }

        public Task<IEnumerable<Maquina>> ListarMaquinasAsync() => _maquinaRepository.ListarMaquinasAsync();

        public Task<Maquina> BuscarMaquinaPorIdAsync(int id) => _maquinaRepository.BuscarMaquinaPorIdAsync(id);

        public Task AdicionarAsync(Maquina maquina) => _maquinaRepository.AdicionarAsync(maquina);

        public Task AtualizarAsync(Maquina maquina) => _maquinaRepository.AtualizarAsync(maquina);
    }
}
