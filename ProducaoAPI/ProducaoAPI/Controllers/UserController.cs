using Microsoft.AspNetCore.Mvc;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Services.Interfaces;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        ///<summary>
        ///Registrar um novo usuário
        ///</summary>
        ///<param name="request">Objeto com os dados do usuário a ser criado.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost("Registrar")]
        public async Task<ActionResult<User>> CadastrarUsuario(UserRequest request)
        {
            var usuario = await _userService.Criar(request);
            return Ok(usuario);
        }

        ///<summary>
        ///Efetuar login no sistema
        ///</summary>
        ///<param name="request">Objeto com os dados do usuário.</param>
        ///<response code="200">Sucesso</response>
        ///<response code="400">Dados inválidos</response>
        ///<response code="404">Usuário não encontrado</response>
        ///<response code="500">Erro de servidor</response>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            await _userService.Authenticate(request.email, request.password);
            var usuario = await _userService.BuscarPorEmail(request.email);
            var token = _userService.GenerateToken(usuario.Id, usuario.Email, usuario.Nome, usuario.Cargo);
            return Ok(new { token });
        }
    }
}