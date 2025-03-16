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
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> ListarProdutos()
        {
            var produtos = await _produtoServices.ListarProdutosAtivos();
            return Ok(_produtoServices.EntityListToResponseList(produtos));
        }

        /// <summary>
        /// Obter produto por ID
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
            return Ok(_produtoServices.EntityToResponse(produto));
        }

        /// <summary>
        /// Criar um novo produto
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CadastrarProduto(ProdutoRequest request)
        {
            var produto = await _produtoServices.AdicionarAsync(request);
            return Ok(_produtoServices.EntityToResponse(produto));
        }

        /// <summary>
        /// Atualizar um produto
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(int id, ProdutoRequest request)
        {
           var produto = await _produtoServices.AtualizarAsync(id, request);
            return Ok(_produtoServices.EntityToResponse(produto));
        }

        /// <summary>
        /// Inativar um produto
        /// </summary>
        ///<response code="200">Sucesso</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoResponse>> InativarProduto(int id)
        {
            var produto = await _produtoServices.InativarProduto(id);
            return Ok(_produtoServices.EntityToResponse(produto));
        }
    }
}
