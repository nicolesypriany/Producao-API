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
        private readonly IProdutoRepository _produtoRepository;

        public FormaServices(IFormaRepository formaRepository, IMaquinaRepository maquinaRepository, IMaquinaService maquinaService, IProdutoService produtoService, IProdutoRepository produtoRepository)
        {
            _formaRepository = formaRepository;
            _maquinaRepository = maquinaRepository;
            _maquinaService = maquinaService;
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
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

        public async Task ValidarDados(FormaRequest request)
        {
            var formas = await _formaRepository.ListarFormasAsync();
            var nomeFormas = new List<string>();
            foreach (var forma in formas)
            {
                nomeFormas.Add(forma.Nome);
            }

            if (nomeFormas.Contains(request.Nome)) throw new ArgumentException("Já existe uma forma com este nome!");

            if (string.IsNullOrWhiteSpace(request.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio.");
            if (request.PecasPorCiclo < 1) throw new ArgumentException("O número de peças por ciclo deve ser maior do que 0.");

            var produto = await _produtoRepository.BuscarProdutoPorIdAsync(request.ProdutoId);

            foreach (var maquina in request.Maquinas)
            {
                await _maquinaRepository.BuscarMaquinaPorIdAsync(maquina.Id);
            }
        }
    }
}
