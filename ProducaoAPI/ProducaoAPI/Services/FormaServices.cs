using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class FormaServices : IFormaService
    {
        private readonly IFormaRepository _formaRepository;
        private readonly IMaquinaRepository _maquinaRepository;

        public FormaServices(IFormaRepository formaRepository, IMaquinaRepository maquinaRepository)
        {
            _formaRepository = formaRepository;
            _maquinaRepository = maquinaRepository;
        }

        public FormaResponse EntityToResponse(Forma forma)
        {
            return new FormaResponse(forma.Id, forma.Nome, ProdutoServices.EntityToResponse(forma.Produto), forma.PecasPorCiclo, MaquinaServices.EntityListToResponseList(forma.Maquinas), forma.Ativo);
        }

        public ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma)
        {
            return forma.Select(f => EntityToResponse(f)).ToList();
        }

        public List<Maquina> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas)
        {
            var maquinasSelecionadas = new List<Maquina>();

            foreach (var maquina in maquinas)
            {
                var maquinaSelecionada = _maquinaRepository.BuscarPorID(maquina.Id);
                maquinasSelecionadas.Add(maquinaSelecionada);
            }

            return maquinasSelecionadas;
        }

        public Task<IEnumerable<Forma>> ListarFormasAsync() => _formaRepository.ListarFormasAsync();

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public Task AdicionarAsync(Forma forma) => _formaRepository.AdicionarAsync(forma);

        public Task AtualizarAsync(Forma forma) => _formaRepository.AtualizarAsync(forma);
    }
}
