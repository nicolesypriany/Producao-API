using ProducaoAPI.Data;
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

        public Task<User> Atualizar(User user)
        {
            throw new NotImplementedException();
        }

        public async Task Criar(User user)
        {
            _context.Usuarios.Add(user);
            _context.SaveChanges();
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
    }
}
