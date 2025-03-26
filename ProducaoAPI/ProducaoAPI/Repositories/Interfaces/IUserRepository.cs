using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Criar(User user);
        Task<User> Atualizar(User user);
        Task<User> Inativar (int id);
        Task<User> Selecionar(int id);
        Task<IEnumerable<User>> SelecionarTodos();
    }
}
