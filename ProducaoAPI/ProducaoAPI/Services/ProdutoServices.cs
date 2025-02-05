using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Services
{
    public class ProdutoServices : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoServices(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public ProdutoResponse EntityToResponse(Produto produto)
        {
            return new ProdutoResponse(produto.Id, produto.Nome, produto.Medidas, produto.Unidade, produto.PecasPorUnidade, produto.Ativo);
        }

        public ICollection<ProdutoResponse> EntityListToResponseList(IEnumerable<Produto> produto)
        {
            return produto.Select(f => EntityToResponse(f)).ToList();
        }

        public Task<IEnumerable<Produto>> ListarProdutosAsync() => _produtoRepository.ListarProdutosAsync();

        public Task<Produto> BuscarProdutoPorIdAsync(int id) => _produtoRepository.BuscarProdutoPorIdAsync(id);

        public Task AdicionarAsync(Produto produto) => _produtoRepository.AdicionarAsync(produto);

        public Task AtualizarAsync(Produto produto) => _produtoRepository.AtualizarAsync(produto);

        public async Task ValidarDados(ProdutoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio.");

            var produtos = await _produtoRepository.ListarProdutosAsync();
            var nomeProdutos = new List<string>();
            foreach (var produto in produtos)
            {
                nomeProdutos.Add(produto.Nome);
            }

            if (nomeProdutos.Contains(request.Nome)) throw new ArgumentException("Já existe um produto com este nome!");
            if (string.IsNullOrWhiteSpace(request.Medidas)) throw new ArgumentException("O campo \"Medidas\" não pode estar vazio.");
            if (string.IsNullOrWhiteSpace(request.Unidade)) throw new ArgumentException("O campo \"Unidade\" não pode estar vazio.");
            if (request.Unidade.Length > 5) throw new ArgumentException("A sigla da unidade não pode ter mais de 5 caracteres.");
            if (request.PecasPorUnidade < 1) throw new ArgumentException("O número de peças por unidade deve ser maior do que 0.");
        }
    }
}
