using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;

namespace ProducaoAPI.Test
{
    public class ContextoFixture
    {
        public ProducaoContext Context { get; }
        public ContextoFixture()
        {
            var options = new DbContextOptionsBuilder<ProducaoContext>()
               .UseNpgsql("Host=localhost;Port=5433;Database=producao-api;Username=postgres;Password=admin")
               .Options;

            Context = new ProducaoContext(options);
        }
    }
}
