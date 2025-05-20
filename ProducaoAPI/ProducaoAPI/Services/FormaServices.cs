using ProducaoAPI.Extensions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;

namespace ProducaoAPI.Services
{
    public class FormaServices : IFormaService
    {
        private readonly IFormaRepository _formaRepository;
        private readonly IMaquinaService _maquinaService;
        private readonly IProdutoService _produtoService;
        private readonly ILogServices _logServices;

        public FormaServices(IFormaRepository formaRepository, IMaquinaService maquinaService, IProdutoService produtoService, ILogServices logServices)
        {
            _formaRepository = formaRepository;
            _maquinaService = maquinaService;
            _produtoService = produtoService;
            _logServices = logServices;
        }

        public async Task<FormaResponse> EntityToResponse(Forma forma)
        {
            var produto = await _produtoService.BuscarProdutoPorIdAsync(forma.ProdutoId);
            var produtoResponse = produto.MapToResponse();
            return new FormaResponse(forma.Id, forma.Nome, produtoResponse, forma.PecasPorCiclo, forma.Ativo);
        }

        public async Task<ICollection<FormaResponse>> EntityListToResponseList(IEnumerable<Forma> formas)
        {
            var responseList = new List<FormaResponse>();
            foreach (var forma in formas)
            {
                var response = await EntityToResponse(forma);
                responseList.Add(response);
            }
            return responseList;
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
            await ValidarRequest(true, request);
            var forma = new Forma(request.Nome, request.ProdutoId, request.PecasPorCiclo);
            await _formaRepository.AdicionarAsync(forma);
            await _logServices.CriarLogAdicionar(typeof(Forma), forma.Id);
            return forma;
        }

        public async Task<Forma> AtualizarAsync(int id, FormaRequest request)
        {
            var forma = await BuscarFormaPorIdAsync(id);
            await ValidarRequest(false, request, forma.Nome);

            await _logServices.CriarLogAtualizar(
                typeof(Forma),
                typeof(FormaRequest),
                forma,
                request,
                forma.Id
            );

            forma.Nome = request.Nome;
            forma.ProdutoId = request.ProdutoId;
            forma.PecasPorCiclo = request.PecasPorCiclo;

            await _formaRepository.AtualizarAsync(forma);
            return forma;
        }

        public async Task<Forma> InativarForma(int id)
        {
            var forma = await BuscarFormaPorIdAsync(id);
            await _logServices.CriarLogInativar(typeof(Forma), forma.Id);
            forma.Ativo = false;
            await _formaRepository.AtualizarAsync(forma);
            return forma;
        }

        private async Task ValidarRequest(bool Cadastrar, FormaRequest request, string nomeAtual = "")
        {
            var nomeFormas = await _formaRepository.ListarNomes();

            ValidarCampos.Nome(Cadastrar, nomeFormas, request.Nome, nomeAtual);
            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.Inteiro(request.PecasPorCiclo, "Peças por Ciclo");
            await ValidarProduto(request.ProdutoId);
        }

        private async Task ValidarProduto(int id)
        {
            await _produtoService.BuscarProdutoPorIdAsync(id);
        }
    }
}
