using Bogus;
using ProducaoAPI.Models;
using System.Net;

namespace ProducaoAPI.IntegrationTests
{
    public class Maquina_DELETE : IClassFixture<ProducaoAPIWebApplicationFactory>
    {
        private readonly ProducaoAPIWebApplicationFactory app;

        public Maquina_DELETE(ProducaoAPIWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task InativarMaquina()
        {
            //arrange
            var maquinaExistente = app.Context.Maquinas.FirstOrDefault();
            if (maquinaExistente is null)
            {
                var fakerMaquina = new Faker<Maquina>().CustomInstantiator(f => new Maquina(
                f.Random.Word(),
                f.Random.Word()));

                maquinaExistente = fakerMaquina.Generate();
                app.Context.Maquinas.Add(maquinaExistente);
                app.Context.SaveChanges();
            }

            var client = app.CreateClient();

            //act
            var response = await client.DeleteAsync($"/Maquina/{maquinaExistente.Id}");

            //assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
