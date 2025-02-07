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
            try
            {
                var produtos = await _produtoServices.ListarProdutosAtivos();
                if (produtos == null) return NotFound();
                return Ok(_produtoServices.EntityListToResponseList(produtos));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Obter produto por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            try
            {
                var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
                return Ok(_produtoServices.EntityToResponse(produto));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Criar um novo produto
        /// </summary>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Request incorreto</response>
        /// <response code="401">Acesso negado</response>
        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CadastrarProduto(ProdutoRequest request)
        {
            try
            {
                await _produtoServices.ValidarDados(request);
                var produto = new Produto(request.Nome, request.Medidas, request.Unidade, request.PecasPorUnidade);
                await _produtoServices.AdicionarAsync(produto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar um produto
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(int id, ProdutoRequest request)
        {
            try
            {
                await _produtoServices.ValidarDados(request);
                var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);

                produto.Nome = request.Nome;
                produto.Medidas = request.Medidas;
                produto.Unidade = request.Unidade;
                produto.PecasPorUnidade = request.PecasPorUnidade;

                await _produtoServices.AtualizarAsync(produto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Inativar um produto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoResponse>> InativarProduto(int id)
        {
            try
            {
                var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
                produto.Ativo = false;

                await _produtoServices.AtualizarAsync(produto);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
