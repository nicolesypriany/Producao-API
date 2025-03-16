using ProducaoAPI.Models;
using System.Net.Http.Json;

namespace ProducaoAPI.IntegrationTests
{
    public class Maquina_GET : IClassFixture<ProducaoAPIWebApplicationFactory>
    {
        private readonly ProducaoAPIWebApplicationFactory app;

        public Maquina_GET(ProducaoAPIWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task RecuperaMaquinaPorId()
        {
            //arrange
            var maquinaExistente = app.Context.Maquinas.FirstOrDefault();
            if (maquinaExistente is null)
            {
                maquinaExistente = new Maquina("teste", "teste");
                app.Context.Maquinas.Add(maquinaExistente);
                app.Context.SaveChanges();
            }

            var client = app.CreateClient();

            //act
            var response = await client.GetFromJsonAsync<Maquina>($"/Maquina/{maquinaExistente.Id}");

            //assert
            Assert.NotNull(response);
            Assert.Equal(maquinaExistente.Nome, response.Nome);
            Assert.Equal(maquinaExistente.Marca, response.Marca);
        }
    }
}
