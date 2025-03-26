using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProducaoAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ProducaoContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;


        public UserService(ProducaoContext context, IUserRepository userRepository, IConfiguration configuration)
        {
            _context = context;
            _userRepository = userRepository;
            _configuration = configuration;

        }

        public Task<User> Atualizar(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Authenticate(string email, string senha)
        {
            var usuario = await _context.Usuarios.Where(u => u.Email.ToUpper() == email.ToUpper()).FirstOrDefaultAsync();

            if (usuario is null) return false;

            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i]) return false;
            }

            return true;
        }

        public async Task<User> BuscarUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<User> Criar(UserRequest request)
        {
            var usuario = new User(request.nome, request.email);

            if (request.password != null)
            {
                using var hmac = new HMACSHA512();
                byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.password));
                byte[] passwordSalt = hmac.Key;

                usuario.AlterarSenha(passwordHash, passwordSalt);
            }

            await _userRepository.Criar(usuario);
            return usuario;
        }

        public Task<User> Inativar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Selecionar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> SelecionarTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string email)
        {
            var usuario = await _context.Usuarios.Where(u => u.Email.ToUpper() == email.ToUpper()).FirstOrDefaultAsync();

            if (usuario is null) return false;

            return true;
        }

        public string GenerateToken(int id, string email)
        {
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("email", email.ToLower()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var privateKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_configuration["jwt:secretKey"]));

            var credentials = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(7);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["jwt:issuer"],
                audience: _configuration["jwt:audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
