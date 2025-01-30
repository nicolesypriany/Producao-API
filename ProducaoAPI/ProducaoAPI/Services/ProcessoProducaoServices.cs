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

        public ProcessoProducaoServices(IProcessoProducaoRepository producaoRepository, IMateriaPrimaRepository materiaPrimaRepository, IFormaRepository formaRepository, IProdutoRepository produtoRepository, IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository, IProducaoMateriaPrimaService producaoService)
        {
            _producaoRepository = producaoRepository;
            _materiaPrimaRepository = materiaPrimaRepository;
            _formaRepository = formaRepository;
            _produtoRepository = produtoRepository;
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
            _producaoService = producaoService;
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

        public List<ProcessoProducaoMateriaPrima> CriarProducoesMateriasPrimas(ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId)
        {
            var producoesMateriasPrimas = new List<ProcessoProducaoMateriaPrima>();

            foreach (var materiaPrima in materiasPrimas)
            {
                var materiaPrimaSelecionada = _materiaPrimaRepository.BuscarMateriaPorIdAsync(materiaPrima.Id);
                var producaoMateriaPrima = new ProcessoProducaoMateriaPrima(ProducaoId, materiaPrimaSelecionada.Id, materiaPrima.Quantidade);
                producoesMateriasPrimas.Add(producaoMateriaPrima);
            }
            return (producoesMateriasPrimas);
        }

        public async Task CalcularProducao(int producaoId)
        {
            var producao = await _producaoRepository.BuscarProducaoPorIdAsync(producaoId);
            //var producao = context.Producoes
            //    .Include(p => p.ProducaoMateriasPrimas)
            //    .ThenInclude(p => p.MateriaPrima)
            //    .FirstOrDefault(p => p.Id == producaoId);

            var forma = await _formaRepository.BuscarFormaPorIdAsync(producao.FormaId);

            //var forma = context.Formas.FirstOrDefault(f => f.Id == producao.FormaId);

            var produto = await _produtoRepository.BuscarProdutoPorIdAsync(producao.ProdutoId);

            //var produto = context.Produtos.FirstOrDefault(p => p.Id == producao.ProdutoId);

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

        public Task<IEnumerable<ProcessoProducao>> ListarProducoesAsync() => _producaoRepository.ListarProducoesAsync();

        public Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id) => _producaoRepository.BuscarProducaoPorIdAsync(id);

        public Task AdicionarAsync(ProcessoProducao producao) => _producaoRepository.AdicionarAsync(producao);

        public Task AtualizarAsync(ProcessoProducao producao) => _producaoRepository.AtualizarAsync(producao);

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public Task AdicionarProducaoMateriaAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);
    }
}
