using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {
        private readonly ProducaoContext _context;
        public ProdutoController(ProducaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> ListarProdutos()
        {
            var produtos = await _context.Produtos.ToListAsync();
            if (produtos == null) return NotFound();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> BuscarProdutoPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> CadastrarProduto(ProdutoRequest req)
        {
            var produto = new Produto(req.Nome, req.Medidas, req.Unidade, req.PecasPorUnidade);
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Produto>> AtualizarProduto(int id, ProdutoRequest req)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            produto.Nome = req.Nome;
            produto.Medidas = req.Medidas;
            produto.Unidade = req.Unidade;
            produto.PecasPorUnidade = req.PecasPorUnidade;

            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Produto>> InativarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            produto.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Produto>> DeletarProduto(int id)
        //{
        //    var produto = await _context.Produtos.FindAsync(id);
        //    if (produto == null) return NotFound();

        //    _context.Produtos.Remove(produto);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}
