using Microsoft.IdentityModel.Tokens;
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

        public FormaServices(IFormaRepository formaRepository, IMaquinaService maquinaService, IProdutoService produtoService)
        {
            _formaRepository = formaRepository;
            _maquinaService = maquinaService;
            _produtoService = produtoService;
        }

        public async Task<FormaResponse> EntityToResponse(Forma forma)
        {
            var produto = await _produtoService.BuscarProdutoPorIdAsync(forma.ProdutoId);
            var produtoResponse = await _produtoService.EntityToResponse(produto);
            if (forma.Maquinas.IsNullOrEmpty())
            {
                List<MaquinaResponse> maquinas = [];
                return new FormaResponse(forma.Id, forma.Nome, produtoResponse, forma.PecasPorCiclo, maquinas, forma.Ativo);
            } else
            {
                var maquinas = _maquinaService.EntityListToResponseList(forma.Maquinas);
                return new FormaResponse(forma.Id, forma.Nome, produtoResponse, forma.PecasPorCiclo, maquinas, forma.Ativo);
            }
        }

        public async Task<ICollection<FormaResponse>> EntityListToResponseList(IEnumerable<Forma> forma)
        {
            var responseList = new List<FormaResponse>();
            foreach (var f in forma)
            {
                var response = await EntityToResponse(f);
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
            if (request.Maquinas.IsNullOrEmpty())
            {
                var forma = new Forma(request.Nome, request.ProdutoId, request.PecasPorCiclo);
                await _formaRepository.AdicionarAsync(forma);
                return forma;
            }
            else
            {
                var maquinas = await FormaMaquinaRequestToEntity(request.Maquinas);
                var forma = new Forma(request.Nome, request.ProdutoId, request.PecasPorCiclo, maquinas);
                await _formaRepository.AdicionarAsync(forma);
                return forma;
            }
        }

        public async Task<Forma> AtualizarAsync(int id, FormaRequest request)
        {
            var forma = await BuscarFormaPorIdAsync(id);
            await ValidarRequest(false, request, forma.Nome);

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

        private async Task ValidarRequest(bool Cadastrar, FormaRequest request, string nomeAtual = "")
        {
            var nomeFormas = await _formaRepository.ListarNomes();

            ValidarCampos.Nome(Cadastrar, nomeFormas, request.Nome, nomeAtual);
            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.Inteiro(request.PecasPorCiclo, "Peças por Ciclo");
            await ValidarProduto(request.ProdutoId);
            await ValidarMaquinas(request.Maquinas);
        }

        private async Task ValidarProduto(int id)
        {
            await _produtoService.BuscarProdutoPorIdAsync(id);
        }

        private async Task ValidarMaquinas(ICollection<FormaMaquinaRequest> maquinas)
        {
            foreach (var maquina in maquinas) await _maquinaService.BuscarMaquinaPorIdAsync(maquina.Id);
        }
    }
}
