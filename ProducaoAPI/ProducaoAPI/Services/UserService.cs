using Microsoft.IdentityModel.Tokens;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProducaoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> Authenticate(string email, string senha)
        {
            var usuario = await _userRepository.BuscarPorEmail(email);
            if (usuario is null) throw new NotFoundException("Usuário não encontrado!");

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i]) throw new BadRequestException("Senha incorreta!");
            }

            return true;
        }

        public async Task<User> Criar(UserRequest request)
        {
            await ValidarRequest(request);
            var usuario = new User(request.Nome, request.Email);

            using var hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
            byte[] passwordSalt = hmac.Key;

            usuario.AlterarSenha(passwordHash, passwordSalt);

            await _userRepository.Criar(usuario);
            return usuario;
        }

        public Task<User> BuscarPorId(int id) => _userRepository.BuscarPorId(id);

        public Task<User> BuscarPorEmail(string email) => _userRepository.BuscarPorEmail(email);

        public Task<IEnumerable<User>> ListarTodos() => _userRepository.ListarTodos();

        public string GenerateToken(int id, string email, string nomeUsuario, string cargo)
        {
            var expiration = DateTime.UtcNow.AddMinutes(30);
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("email", email.ToLower()),
                new Claim("nome", nomeUsuario),
                new Claim(ClaimTypes.Role, cargo),
                new Claim("expiracao", expiration.ToString()),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_configuration["jwt:secretKey"]));
            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["jwt:issuer"],
                audience: _configuration["jwt:audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task ValidarRequest(UserRequest request)
        {
            ValidarCampos.String(request.Email, "Email");
            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.String(request.Password, "Senha");

            var emailExiste = await _userRepository.VerificarSeEmailExiste(request.Email);
            if (emailExiste) throw new BadRequestException("Email já cadastrado!");
        }
    }
}
