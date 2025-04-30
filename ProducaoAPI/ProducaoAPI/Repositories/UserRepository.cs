using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProducaoContext _context;

        public UserRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task Criar(User user)
        {
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> BuscarPorId(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null) throw new NotFoundException("Usuário não encontrado.");
            return usuario;
        }

        public async Task<User> BuscarPorEmail(string email)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email.ToUpper() == email.ToUpper())
                .FirstOrDefaultAsync();

            if (usuario is null) throw new NotFoundException("Usuário não encontrado.");
            return usuario;
        }

        public async Task<IEnumerable<User>> ListarTodos()
        {
            var usuarios = await _context.Usuarios
                .ToListAsync();

            if (usuarios == null || usuarios.Count == 0) throw new NotFoundException("Nenhum usuário encontrado.");
            return usuarios;
        }
    }
}
