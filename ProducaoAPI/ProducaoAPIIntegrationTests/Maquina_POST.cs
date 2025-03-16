using Bogus;
using ProducaoAPI.Models;
using System.Net;
using System.Net.Http.Json;

namespace ProducaoAPI.IntegrationTests
{
    public class Maquina_POST : IClassFixture<ProducaoAPIWebApplicationFactory>
    {
        private readonly ProducaoAPIWebApplicationFactory app;
        public Maquina_POST(ProducaoAPIWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task CadastraMaquina()
        {
            //arrange
            var fakerMaquina = new Faker<Maquina>().CustomInstantiator(f => new Maquina(
                f.Random.Word(),
                f.Random.Word()));

            var maquina = fakerMaquina.Generate();
            var client = app.CreateClient();

            //act
            var response = await client.PostAsJsonAsync("/Maquina", maquina);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
