using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Criar(User user);
        Task<User> BuscarPorId(int id);
        Task<User> BuscarPorEmail(string email);
        Task<IEnumerable<User>> ListarTodos();
    }
}
