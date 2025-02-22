using ProducaoAPI.Exceptions;
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
        private readonly IMaquinaService _maquinaService;
        private readonly IProdutoService _produtoService;

        public FormaServices(IFormaRepository formaRepository, IMaquinaService maquinaService, IProdutoService produtoService)
        {
            _formaRepository = formaRepository;
            _maquinaService = maquinaService;
            _produtoService = produtoService;
        }

        public FormaResponse EntityToResponse(Forma forma)
        {
            var produto = _produtoService.EntityToResponse(forma.Produto);
            var maquinas = _maquinaService.EntityListToResponseList(forma.Maquinas);
            return new FormaResponse(forma.Id, forma.Nome, produto, forma.PecasPorCiclo, maquinas, forma.Ativo);
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
                var maquinaSelecionada = _maquinaService.BuscarMaquinaPorIdAsync(maquina.Id);
                var maq = await maquinaSelecionada;
                maquinasSelecionadas.Add(maq);
            }

            return maquinasSelecionadas;
        }

        public Task<IEnumerable<Forma>> ListarFormasAtivas() => _formaRepository.ListarFormasAtivas();

        public Task<IEnumerable<Forma>> ListarTodasFormas() => _formaRepository.ListarTodasFormas();

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public async Task<Forma> AdicionarAsync(FormaRequest request)
        {
            await ValidarDadosParaCadastrar(request);
            var maquinas = await FormaMaquinaRequestToEntity(request.Maquinas);
            var forma = new Forma(request.Nome, request.ProdutoId, request.PecasPorCiclo, maquinas);
            await _formaRepository.AdicionarAsync(forma);
            return forma;
        }

        public async Task<Forma> AtualizarAsync(int id, FormaRequest request)
        {
            await ValidarDadosParaAtualizar(request, id);
            var forma = await BuscarFormaPorIdAsync(id);

            var maquinas = await FormaMaquinaRequestToEntity(request.Maquinas);

            forma.Nome = request.Nome;
            forma.ProdutoId = request.ProdutoId;
            forma.PecasPorCiclo = request.PecasPorCiclo;
            forma.Maquinas = maquinas;

            await _formaRepository.AtualizarAsync(forma);
            return forma;
        }

        public async Task<Forma> InativarForma(int id)
        {
            var forma = await BuscarFormaPorIdAsync(id);
            forma.Ativo = false;
            await _formaRepository.AtualizarAsync(forma);
            return forma;
        }

        public async Task ValidarDadosParaCadastrar(FormaRequest request)
        {
            var formas = await _formaRepository.ListarTodasFormas();
            var nomeFormas = new List<string>();
            foreach (var forma in formas)
            {
                nomeFormas.Add(forma.Nome);
            }

            if (nomeFormas.Contains(request.Nome)) throw new BadRequestException("Já existe uma forma com este nome!");
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new BadRequestException("O campo \"Nome\" não pode estar vazio.");
            if (request.PecasPorCiclo < 1) throw new BadRequestException("O número de peças por ciclo deve ser maior do que 0.");

            await _produtoService.BuscarProdutoPorIdAsync(request.ProdutoId);

            foreach (var maquina in request.Maquinas)
            {
                await _maquinaService.BuscarMaquinaPorIdAsync(maquina.Id);
            }
        }

        public async Task ValidarDadosParaAtualizar(FormaRequest request, int id)
        {
            var formaAtualizada = await _formaRepository.BuscarFormaPorIdAsync(id);

            var formas = await _formaRepository.ListarTodasFormas();
            var nomeFormas = new List<string>();
            foreach (var forma in formas)
            {
                nomeFormas.Add(forma.Nome);
            }

            if (nomeFormas.Contains(request.Nome) && formaAtualizada.Nome != request.Nome) throw new BadRequestException("Já existe uma forma com este nome!");
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new BadRequestException("O campo \"Nome\" não pode estar vazio.");
            if (request.PecasPorCiclo < 1) throw new BadRequestException("O número de peças por ciclo deve ser maior do que 0.");

            await _produtoService.BuscarProdutoPorIdAsync(request.ProdutoId);

            foreach (var maquina in request.Maquinas)
            {
                await _maquinaService.BuscarMaquinaPorIdAsync(maquina.Id);
            }
        }
    }
}
