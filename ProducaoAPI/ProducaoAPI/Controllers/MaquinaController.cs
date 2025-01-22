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

    public class MaquinaController : Controller
    {
        private readonly IMaquinaService _maquinaService;

        public MaquinaController(IMaquinaService maquinaService)
        {
            _maquinaService = maquinaService;
        }

        /// <summary>
        /// Obter máquinas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaquinaResponse>>> ListarMaquinas()
        {
            var maquinas = await _maquinaService.ListarMaquinasAsync();
            if (maquinas == null) return NotFound();
            return Ok(_maquinaService.EntityListToResponseList(maquinas));
        }

        /// <summary>
        /// Obter máquina por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MaquinaResponse>> BuscarMaquinaPorId(int id)
        {
            var maquina = await _maquinaService.BuscarMaquinaPorIdAsync(id);
            if (maquina == null) return NotFound();
            return Ok(_maquinaService.EntityToResponse(maquina));
        }

        /// <summary>
        /// Criar uma nova máquina
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<MaquinaResponse>> CadastrarMaquina(MaquinaRequest req)
        {
            var maquina = new Maquina(req.Nome, req.Marca);
            await _maquinaService.AdicionarAsync(maquina);
            return Ok(maquina);
        }

        /// <summary>
        /// Atualizar uma máquina
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MaquinaResponse>> AtualizarMaquina(int id, MaquinaRequest req)
        {
            var maquina = await _maquinaService.BuscarMaquinaPorIdAsync(id);
            if (maquina == null) return NotFound();

            maquina.Nome = req.Nome;
            maquina.Marca = req.Marca;

            await _maquinaService.AtualizarAsync(maquina);
            return Ok(maquina);
        }

        /// <summary>
        /// Inativar uma máquina
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaquinaResponse>> InativarMaquina(int id)
        {
            var maquina = await _maquinaService.BuscarMaquinaPorIdAsync(id);
            if (maquina == null) return NotFound();
            maquina.Ativo = false;

            await _maquinaService.AtualizarAsync(maquina);
            return Ok(maquina);
        }
    }
}
