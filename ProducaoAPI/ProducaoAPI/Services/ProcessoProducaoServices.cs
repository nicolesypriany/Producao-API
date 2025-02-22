using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class ProcessoProducaoServices : IProcessoProducaoService
    {
        private readonly IProcessoProducaoRepository _producaoRepository;
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;
        private readonly IFormaRepository _formaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProducaoMateriaPrimaRepository _producaoMateriaPrimaRepository;
        private readonly IProducaoMateriaPrimaService _producaoService;
        private readonly IMaquinaRepository _maquinaRepository;

        public ProcessoProducaoServices(IProcessoProducaoRepository producaoRepository, IMateriaPrimaRepository materiaPrimaRepository, IFormaRepository formaRepository, IProdutoRepository produtoRepository, IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository, IProducaoMateriaPrimaService producaoService, IMaquinaRepository maquinaRepository)
        {
            _producaoRepository = producaoRepository;
            _materiaPrimaRepository = materiaPrimaRepository;
            _formaRepository = formaRepository;
            _produtoRepository = produtoRepository;
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
            _producaoService = producaoService;
            _maquinaRepository = maquinaRepository;
        }

        public ProcessoProducaoResponse EntityToResponse(ProcessoProducao producao)
        {
            var prod = _producaoService.EntityListToResponseList(producao.ProducaoMateriasPrimas);
            return new ProcessoProducaoResponse(producao.Id, producao.Data, producao.MaquinaId, producao.FormaId, producao.Ciclos, prod, producao.QuantidadeProduzida, producao.CustoUnitario, producao.CustoTotal, producao.Ativo);

        }

        public ICollection<ProcessoProducaoResponse> EntityListToResponseList(IEnumerable<ProcessoProducao> producoes)
        {
            return producoes.Select(m => EntityToResponse(m)).ToList();
        }

        public async Task<List<ProcessoProducaoMateriaPrima>> CriarProducoesMateriasPrimas(ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId)
        {
            var producoesMateriasPrimas = new List<ProcessoProducaoMateriaPrima>();

            foreach (var materiaPrima in materiasPrimas)
            {
                var materiaPrimaSelecionada = await _materiaPrimaRepository.BuscarMateriaPorIdAsync(materiaPrima.Id);
                var producaoMateriaPrima = new ProcessoProducaoMateriaPrima(ProducaoId, materiaPrimaSelecionada.Id, materiaPrima.Quantidade);
                producoesMateriasPrimas.Add(producaoMateriaPrima);
            }
            return producoesMateriasPrimas;
        }

        public async Task CalcularProducao(int producaoId)
        {
            var producao = await _producaoRepository.BuscarProducaoPorIdAsync(producaoId);

            var forma = await _formaRepository.BuscarFormaPorIdAsync(producao.FormaId);

            var produto = await _produtoRepository.BuscarProdutoPorIdAsync(producao.ProdutoId);

            double quantidadeProduzida = ((Convert.ToDouble(producao.Ciclos)) * forma.PecasPorCiclo) / produto.PecasPorUnidade;

            double custoTotal = 0;
            foreach (var producaoMateriaPrima in producao.ProducaoMateriasPrimas)
            {
                var total = producaoMateriaPrima.Quantidade * producaoMateriaPrima.MateriaPrima.Preco;
                custoTotal += total;
            }

            producao.QuantidadeProduzida = quantidadeProduzida;
            producao.CustoTotal = custoTotal;
            producao.CustoUnitario = custoTotal / quantidadeProduzida;
            await _producaoRepository.AtualizarAsync(producao);
        }

        public Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas() => _producaoRepository.ListarProducoesAtivas();
        public Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes() => _producaoRepository.ListarTodasProducoes();

        public Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id) => _producaoRepository.BuscarProducaoPorIdAsync(id);

        public async Task<ProcessoProducao> AdicionarAsync(ProcessoProducaoRequest request)
        {

            await ValidarDados(request);
            var forma = await _formaRepository.BuscarFormaPorIdAsync(request.FormaId);
            var producao = new ProcessoProducao(request.Data, request.MaquinaId, request.FormaId, forma.ProdutoId, request.Ciclos);
            await _producaoRepository.AdicionarAsync(producao);

            var producaoMateriasPrimas = await CriarProducoesMateriasPrimas(request.MateriasPrimas, producao.Id);
            foreach (var producaMateriaPrima in producaoMateriasPrimas)
            {
                await _producaoMateriaPrimaRepository.AdicionarAsync(producaMateriaPrima);
            }

            return producao;
        }

        public async Task<ProcessoProducao> AtualizarAsync(int id, ProcessoProducaoRequest request)
        {
            await ValidarDados(request);
            var forma = await _formaRepository.BuscarFormaPorIdAsync(request.FormaId);

            var producao = await BuscarProducaoPorIdAsync(id);

            _producaoService.VerificarProducoesMateriasPrimasExistentes(id, request.MateriasPrimas);

            producao.Data = request.Data;
            producao.MaquinaId = request.MaquinaId;
            producao.FormaId = request.FormaId;
            producao.ProdutoId = forma.ProdutoId;
            producao.Ciclos = producao.Ciclos;

            await _producaoRepository.AtualizarAsync(producao);
            return producao;
        }

        public async Task<ProcessoProducao> InativarProducao(int id)
        {
            var producao = await BuscarProducaoPorIdAsync(id);
            producao.Ativo = false;
            await _producaoRepository.AtualizarAsync(producao);
            return producao;
        }

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public Task AdicionarProducaoMateriaAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);

        public async Task ValidarDados(ProcessoProducaoRequest request)
        {
            if (request.Ciclos <= 0) throw new BadRequestException("O número de ciclos deve ser maior que 0.");

            await _maquinaRepository.BuscarMaquinaPorIdAsync(request.MaquinaId);
            await _formaRepository.BuscarFormaPorIdAsync(request.FormaId);

            foreach (var materiaPrima in request.MateriasPrimas)
            {
                await _materiaPrimaRepository.BuscarMateriaPorIdAsync(materiaPrima.Id);
                if (materiaPrima.Quantidade <= 0) throw new BadRequestException("A quantidade de matéria-prima deve ser maior que 0.");
            }
        }
    }
}
