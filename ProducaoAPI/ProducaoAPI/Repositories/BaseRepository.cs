using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ProducaoContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ProducaoContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Adicionar(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public T BuscarPorID(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}