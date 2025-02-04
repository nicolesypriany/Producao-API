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
    public class FormaController : Controller
    {
        private readonly IFormaService _formaServices;

        public FormaController(IFormaService formaServices)
        {
            _formaServices = formaServices;
        }

        /// <summary>
        /// Obter formas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormaResponse>>> ListarFormas()
        {
            try
            {
                var formas = await _formaServices.ListarFormasAsync();
                if (formas == null) return NotFound();
                return Ok(_formaServices.EntityListToResponseList(formas));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        /// <summary>
        /// Obter forma por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaResponse>> BuscarFormaPorId(int id)
        {
            try
            {
                var forma = await _formaServices.BuscarFormaPorIdAsync(id);
                if (forma == null) return NotFound();
                return Ok(_formaServices.EntityToResponse(forma));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Criar uma nova forma
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FormaResponse>> CadastrarForma(FormaRequest req)
        {
            try
            {
                var forma = new Forma(req.Nome, req.ProdutoId, req.PecasPorCiclo);
                await _formaServices.AdicionarAsync(forma);
                return Ok(forma);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar uma forma
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<FormaResponse>> AtualizarForma(int id, FormaRequest req)
        {
            try
            {
                var forma = await _formaServices.BuscarFormaPorIdAsync(id);
                if (forma == null) return NotFound();

                var maquinas = await _formaServices.FormaMaquinaRequestToEntity(req.Maquinas);

                forma.Nome = req.Nome;
                forma.ProdutoId = req.ProdutoId;
                forma.PecasPorCiclo = req.PecasPorCiclo;
                forma.Maquinas = maquinas;

                await _formaServices.AtualizarAsync(forma);
                return Ok(forma);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Inativar uma forma
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<FormaResponse>> InativarForma(int id)
        {
            try
            {
                var forma = await _formaServices.BuscarFormaPorIdAsync(id);
                if (forma == null) return NotFound();
                forma.Ativo = false;

                await _formaServices.AtualizarAsync(forma);
                return Ok(forma);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}