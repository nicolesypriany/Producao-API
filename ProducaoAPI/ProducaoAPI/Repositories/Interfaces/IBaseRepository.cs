namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T BuscarPorID(int id);
        Task Adicionar(T entity);
        Task Atualizar(T entity);
    }
}
