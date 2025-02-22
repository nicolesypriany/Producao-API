using ProducaoAPI.Exceptions;
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

        public Task<IEnumerable<Produto>> ListarProdutosAtivos() => _produtoRepository.ListarProdutosAtivos();

        public Task<IEnumerable<Produto>> ListarTodosProdutos() => _produtoRepository.ListarTodosProdutos();

        public Task<Produto> BuscarProdutoPorIdAsync(int id) => _produtoRepository.BuscarProdutoPorIdAsync(id);

        public async Task<Produto> AdicionarAsync(ProdutoRequest request)
        {
            await ValidarDadosParaCadastrar(request);
            var produto = new Produto(request.Nome, request.Medidas, request.Unidade, request.PecasPorUnidade);
            await _produtoRepository.AdicionarAsync(produto);
            return produto;
        }

        public async Task<Produto> AtualizarAsync(int id, ProdutoRequest request)
        {
                await ValidarDadosParaAtualizar(request, id);
                var produto = await _produtoRepository.BuscarProdutoPorIdAsync(id);

                produto.Nome = request.Nome;
                produto.Medidas = request.Medidas;
                produto.Unidade = request.Unidade;
                produto.PecasPorUnidade = request.PecasPorUnidade;

                await _produtoRepository.AtualizarAsync(produto);
                return produto;
        }

        public async Task<Produto> InativarProduto(int id)
        {
            var produto = await BuscarProdutoPorIdAsync(id);
            produto.Ativo = false;
            await _produtoRepository.AtualizarAsync(produto);
            return produto;
        }

        public async Task ValidarDadosParaCadastrar(ProdutoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome)) throw new BadRequestException("O campo \"Nome\" não pode estar vazio.");

            var produtos = await _produtoRepository.ListarTodosProdutos();
            var nomeProdutos = new List<string>();
            foreach (var produto in produtos)
            {
                nomeProdutos.Add(produto.Nome);
            }

            if (nomeProdutos.Contains(request.Nome)) throw new BadRequestException("Já existe um produto com este nome!");
            if (string.IsNullOrWhiteSpace(request.Medidas)) throw new BadRequestException("O campo \"Medidas\" não pode estar vazio.");
            if (string.IsNullOrWhiteSpace(request.Unidade)) throw new BadRequestException("O campo \"Unidade\" não pode estar vazio.");
            if (request.Unidade.Length > 5) throw new BadRequestException("A sigla da unidade não pode ter mais de 5 caracteres.");
            if (request.PecasPorUnidade < 1) throw new BadRequestException("O número de peças por unidade deve ser maior do que 0.");
        }

        public async Task ValidarDadosParaAtualizar(ProdutoRequest request, int id)
        {
            var produtoAtualizado = await _produtoRepository.BuscarProdutoPorIdAsync(id);

            if (string.IsNullOrWhiteSpace(request.Nome)) throw new BadRequestException("O campo \"Nome\" não pode estar vazio.");

            var produtos = await _produtoRepository.ListarTodosProdutos();
            var nomeProdutos = new List<string>();
            foreach (var produto in produtos)
            {
                nomeProdutos.Add(produto.Nome);
            }

            if (nomeProdutos.Contains(request.Nome) && produtoAtualizado.Nome != request.Nome) throw new ArgumentException("Já existe um produto com este nome!");

            if (string.IsNullOrWhiteSpace(request.Medidas)) throw new BadRequestException("O campo \"Medidas\" não pode estar vazio.");
            if (string.IsNullOrWhiteSpace(request.Unidade)) throw new BadRequestException("O campo \"Unidade\" não pode estar vazio.");
            if (request.Unidade.Length > 5) throw new BadRequestException("A sigla da unidade não pode ter mais de 5 caracteres.");
            if (request.PecasPorUnidade < 1) throw new BadRequestException("O número de peças por unidade deve ser maior do que 0.");
        }
    }
}
