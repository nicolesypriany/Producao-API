using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FormaController : Controller
    {
        private readonly ProducaoContext _context;
        public FormaController(ProducaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forma>>> ListarFormas()
        {
            var formas = await _context.Formas.Where(m => m.Ativo == true).Include(f => f.Maquinas).Include(f => f.Produto).ToListAsync();
            if (formas == null) return NotFound();
            return Ok(FormaServices.EntityListToResponseList(formas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Forma>> BuscarFormaPorId(int id)
        {
            var forma = await _context.Formas.Include(f => f.Maquinas).Include(f => f.Produto).FirstOrDefaultAsync(f => f.Id == id);
            if (forma == null) return NotFound();
            return Ok(FormaServices.EntityToResponse(forma));
        }

        [HttpPost]
        public async Task<ActionResult<Forma>> CadastrarForma(FormaRequest req)
        {
            var forma = new Forma(req.Nome, req.ProdutoId, req.PecasPorCiclo);
            await _context.Formas.AddAsync(forma);
            await _context.SaveChangesAsync();
            return Ok(forma);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Forma>> AtualizarForma(int id, FormaRequest req)
        {
            var forma = await _context.Formas.Include(f => f.Maquinas).FirstOrDefaultAsync(f => f.Id == id);
            if (forma == null) return NotFound();

            var maquinas = FormaServices.FormaMaquinaRequestToEntity(req.Maquinas, _context);

            forma.Nome = req.Nome;
            forma.ProdutoId = req.ProdutoId;
            forma.PecasPorCiclo = req.PecasPorCiclo;
            forma.Maquinas = maquinas;

            await _context.SaveChangesAsync();
            return Ok(forma);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Forma>> InativarForma(int id)
        {
            var forma = await _context.Formas.FindAsync(id);
            if (forma == null) return NotFound();
            forma.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(forma);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Forma>> DeletarForma(int id)
        //{
        //    var forma = await _context.Formas.FindAsync(id);
        //    if (forma == null) return NotFound();

        //    _context.Formas.Remove(forma);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}