using Bogus;
using ProducaoAPI.Models;
using System.Net;
using System.Net.Http.Json;

namespace ProducaoAPI.IntegrationTests
{
    public class Maquina_PUT : IClassFixture<ProducaoAPIWebApplicationFactory>
    {
        private readonly ProducaoAPIWebApplicationFactory app;

        public Maquina_PUT(ProducaoAPIWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task AtualizarMaquina()
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
            var fakerWord = new Faker().Random.Word();
            maquinaExistente.Nome = fakerWord;
            maquinaExistente.Marca = fakerWord;

            //act
            var response = await client.PutAsJsonAsync($"/Maquina/{maquinaExistente.Id}", maquinaExistente);

            //assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
