using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class ProdutoController : Controller
    {
        private readonly IProdutoService _produtoServices;
        public ProdutoController(IProdutoService produtoServices)
        {
            _produtoServices = produtoServices;
        }

        /// <summary>
        /// Obter produtos
        /// </summary>
        ///// <returns>A lista de produtos cadastrados</returns>
        ///// <response code="200">Sucesso</response>
        ///// <response code="404">Nenhum produto encontrado </response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> ListarProdutos()
        {
            var produtos = await _produtoServices.ListarProdutosAsync();
            if (produtos == null) return NotFound();
            return Ok(_produtoServices.EntityListToResponseList(produtos));
        }

        /// <summary>
        /// Obter produto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(_produtoServices.EntityToResponse(produto));
        }

        /// <summary>
        /// Criar um novo produto
        /// </summary>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Request incorreto</response>
        /// <response code="401">Acesso negado</response>
        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CadastrarProduto(ProdutoRequest req)
        {
            var produto = new Produto(req.Nome, req.Medidas, req.Unidade, req.PecasPorUnidade);
            await _produtoServices.AdicionarAsync(produto);
            return Ok(produto);
        }

        /// <summary>
        /// Atualizar um produto
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(int id, ProdutoRequest req)
        {
            var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
            if (produto == null) return NotFound();

            produto.Nome = req.Nome;
            produto.Medidas = req.Medidas;
            produto.Unidade = req.Unidade;
            produto.PecasPorUnidade = req.PecasPorUnidade;

            await _produtoServices.AtualizarAsync(produto);
            return Ok(produto);
        }

        /// <summary>
        /// Inativar um produto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoResponse>> InativarProduto(int id)
        {
            var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
            if (produto == null) return NotFound();
            produto.Ativo = false;

            await _produtoServices.AtualizarAsync(produto);
            return Ok(produto);
        }
    }
}
