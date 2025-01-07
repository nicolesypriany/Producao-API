using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class MaquinaController : Controller
    {
        private readonly IMaquinaRepository _maquinaRepository;

        public MaquinaController(IMaquinaRepository maquinaRepository)
        {
            _maquinaRepository = maquinaRepository;
        }

        /// <summary>
        /// Obter máquinas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaquinaResponse>>> ListarMaquinas()
        {
            var maquinas = _maquinaRepository.ListarMaquinas();
            if (maquinas == null) return NotFound();
            return Ok(MaquinaServices.EntityListToResponseList(maquinas));
        }

        /// <summary>
        /// Obter máquina por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MaquinaResponse>> BuscarMaquinaPorId(int id)
        {
            var maquina = _maquinaRepository.BuscarPorID(id);
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
            await _maquinaRepository.Adicionar(maquina);
            return Ok(maquina);
        }

        /// <summary>
        /// Atualizar uma máquina
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<MaquinaResponse>> AtualizarMaquina(int id, MaquinaRequest req)
        {
            var maquina = _maquinaRepository.BuscarPorID(id);
            if (maquina == null) return NotFound();

            maquina.Nome = req.Nome;
            maquina.Marca = req.Marca;

            await _maquinaRepository.Atualizar(maquina);
            return Ok(maquina);
        }

        /// <summary>
        /// Inativar uma máquina
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaquinaResponse>> InativarMaquina(int id)
        {
            var maquina = _maquinaRepository.BuscarPorID(id);
            if (maquina == null) return NotFound();
            maquina.Ativo = false;

            await _maquinaRepository.Atualizar(maquina);
            return Ok(maquina);
        }
    }
}
