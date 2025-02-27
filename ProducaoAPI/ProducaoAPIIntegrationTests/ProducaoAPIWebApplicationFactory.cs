using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProducaoAPI.Data;

namespace ProducaoAPI.IntegrationTests
{
    public class ProducaoAPIWebApplicationFactory : WebApplicationFactory<Program>
    {
        public ProducaoContext Context { get; }
        private IServiceScope scope;

        public ProducaoAPIWebApplicationFactory()
        {
            scope = Services.CreateScope();
            Context = scope.ServiceProvider.GetRequiredService<ProducaoContext>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<ProducaoContext>));
                services.AddDbContext<ProducaoContext>(options => options.UseNpgsql("Host=localhost;Port=5433;Database=producao-api;Username=postgres;Password=admin"));
            });

            base.ConfigureWebHost(builder);
        }
    }
}
