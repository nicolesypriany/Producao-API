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
        private readonly IMaquinaService _maquinaService;
        private readonly IProdutoService _produtoService;

        public FormaServices(IFormaRepository formaRepository, IMaquinaRepository maquinaRepository, IMaquinaService maquinaService, IProdutoService produtoService)
        {
            _formaRepository = formaRepository;
            _maquinaRepository = maquinaRepository;
            _maquinaService = maquinaService;
            _produtoService = produtoService;
        }

        public FormaResponse EntityToResponse(Forma forma)
        {
            return new FormaResponse(forma.Id, forma.Nome, _produtoService.EntityToResponse(forma.Produto), forma.PecasPorCiclo, _maquinaService.EntityListToResponseList(forma.Maquinas), forma.Ativo);
        }

        public ICollection<FormaResponse> EntityListToResponseList(IEnumerable<Forma> forma)
        {
            return forma.Select(f => EntityToResponse(f)).ToList();
        }

        public async Task<List<Maquina>> FormaMaquinaRequestToEntity(ICollection<FormaMaquinaRequest> maquinas)
        {
            var maquinasSelecionadas = new List<Maquina>();

            foreach (var maquina in maquinas)
            {
                var maquinaSelecionada = _maquinaRepository.BuscarMaquinaPorIdAsync(maquina.Id);
                var maq = await maquinaSelecionada;
                maquinasSelecionadas.Add(maq);
            }

            return maquinasSelecionadas;
        }

        public Task<IEnumerable<Forma>> ListarFormasAsync() => _formaRepository.ListarFormasAsync();

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public Task AdicionarAsync(Forma forma) => _formaRepository.AdicionarAsync(forma);

        public Task AtualizarAsync(Forma forma) => _formaRepository.AtualizarAsync(forma);
    }
}
