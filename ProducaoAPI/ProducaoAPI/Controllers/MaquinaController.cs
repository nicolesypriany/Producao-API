using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MaquinaController : Controller
    {
        private readonly ProducaoContext _context;

        public MaquinaController(ProducaoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter máquinas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaquinaResponse>>> ListarMaquinas()
        {
            var maquinas = await _context.Maquinas.Where(m => m.Ativo == true).ToListAsync();
            if (maquinas == null) return NotFound();
            return Ok(MaquinaServices.EntityListToResponseList(maquinas));
        }

        /// <summary>
        /// Obter máquina por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MaquinaResponse>> BuscarMaquinaPorId(int id)
        {
            var maquina = await _context.Maquinas.FindAsync(id);
            if (maquina == null) return NotFound();
            return Ok(MaquinaServices.EntityToResponse(maquina));
        }

        /// <summary>
        /// Criar uma nova máquina
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MaquinaResponse>> CadastrarMaquina(MaquinaRequest req)
        {
            var maquina = new Maquina(req.Nome, req.Marca);
            await _context.Maquinas.AddAsync(maquina);
            await _context.SaveChangesAsync();
            return Ok(maquina);
        }

        /// <summary>
        /// Atualizar uma máquina
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MaquinaResponse>> AtualizarMaquina(int id, MaquinaRequest req)
        {
            var maquina = await _context.Maquinas.FindAsync(id);
            if (maquina == null) return NotFound();

            maquina.Nome = req.Nome;
            maquina.Marca = req.Marca;

            await _context.SaveChangesAsync();
            return Ok(maquina);
        }

        /// <summary>
        /// Inativar uma máquina
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaquinaResponse>> InativarMaquina(int id)
        {
            var maquina = await _context.Maquinas.FindAsync(id);
            if (maquina == null) return NotFound();
            maquina.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(maquina);
        }
    }
}
