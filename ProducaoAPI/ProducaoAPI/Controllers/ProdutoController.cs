using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Extensions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> ListarProdutos()
        {
            var produtos = await _produtoServices.ListarProdutosAtivos();
            return Ok(produtos.MapListToResponse());
        }

        /// <summary>
        /// Obter produto por ID
        /// </summary>
        ///<param name="id">ID do produto buscado.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            var produto = await _produtoServices.BuscarProdutoPorIdAsync(id);
            return Ok(produto.MapToResponse());
        }

        /// <summary>
        /// Criar um novo produto
        /// </summary>
        ///<param name="request">Objeto com os dados do produto a ser criado.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CadastrarProduto(ProdutoRequest request)
        {
            var produto = await _produtoServices.AdicionarAsync(request);
            return Ok(produto.MapToResponse());
        }

        /// <summary>
        /// Atualizar um produto
        /// </summary>
        ///<param name="id">ID do produto a ser atualizado.</param>
        ///<param name="request">Objeto com os dados atualizados do produto.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(int id, ProdutoRequest request)
        {
            var produto = await _produtoServices.AtualizarAsync(id, request);
            return Ok(produto.MapToResponse());
        }

        /// <summary>
        /// Inativar um produto
        /// </summary>
        ///<param name="id">ID do produto a ser inativado.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="401">Usuário não autorizado</response>
        ///<response code="404">Nenhum produto encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [Authorize(Roles = "Administrador,Gerente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoResponse>> InativarProduto(int id)
        {
            var produto = await _produtoServices.InativarProduto(id);
            return Ok(produto.MapToResponse());
        }
    }
}
