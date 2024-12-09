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
    public class MateriaPrimaController : Controller
    {
        private readonly ProducaoContext _context;
        public MateriaPrimaController(ProducaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrima>>> ListarMateriasPrimas()
        {
            var materiasPrimas = await _context.MateriasPrimas.Where(m => m.Ativo == true).ToListAsync();
            if (materiasPrimas == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityListToResponseList(materiasPrimas));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrima>> BuscarMateriaPrimaPorId(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();
            return Ok(MateriaPrimaServices.EntityToResponse(materiaPrima));
        }

        [HttpPost]
        public async Task<ActionResult<MateriaPrima>> CadastrarMateriaPrima(MateriaPrimaRequest req)
        {
            var materiaPrima = new MateriaPrima(req.Nome, req.Fornecedor, req.Unidade, req.Preco);
            await _context.MateriasPrimas.AddAsync(materiaPrima);
            await _context.SaveChangesAsync();
            return Ok(materiaPrima);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MateriaPrima>> AtualizarMateriaPrima(int id, MateriaPrimaRequest req)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();

            materiaPrima.Nome = req.Nome;
            materiaPrima.Fornecedor = req.Fornecedor;
            materiaPrima.Unidade = req.Unidade;
            materiaPrima.Preco = req.Preco;

            await _context.SaveChangesAsync();
            return Ok(materiaPrima);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<MateriaPrima>> InativarProduto(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null) return NotFound();
            materiaPrima.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(materiaPrima);
        }
    }
}
