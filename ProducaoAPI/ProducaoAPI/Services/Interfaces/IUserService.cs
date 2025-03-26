using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using System.Reflection.Metadata;

namespace ProducaoAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Criar(UserRequest request);
        Task<User> Atualizar(UserRequest request);
        Task<User> Inativar(int id);
        Task<User> Selecionar(int id);
        Task<IEnumerable<User>> SelecionarTodos();
        Task<bool> Authenticate(string email, string senha);
        Task<User> BuscarUsuarioPorEmail(string email);
        string GenerateToken(int id, string email);
    }
}
