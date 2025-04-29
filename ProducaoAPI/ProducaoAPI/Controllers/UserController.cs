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

        [HttpPost]
        public async Task<ActionResult<User>> CadastrarUsuario(UserRequest request)
        {
            var usuario = await _userService.Criar(request);
            return Ok(usuario);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            await _userService.Authenticate(request.email, request.password);
            var usuario = await _userService.BuscarPorEmail(request.email);
            var token = _userService.GenerateToken(usuario.Id, usuario.Email);
            return Ok(new { token });
        }
    }
}
