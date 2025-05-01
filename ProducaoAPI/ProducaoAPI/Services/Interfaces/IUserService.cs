using ProducaoAPI.Models;
using ProducaoAPI.Requests;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Authenticate(string email, string senha);
        Task<User> Criar(UserRequest request);
        Task<User> BuscarPorId(int id);
        Task<User> BuscarPorEmail(string email);
        Task<IEnumerable<User>> ListarTodos();
        string GenerateToken(int id, string email, string nomeUsuario, string cargo);
    }
}
