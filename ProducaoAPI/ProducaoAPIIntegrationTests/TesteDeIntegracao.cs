namespace ProducaoAPI.IntegrationTests
{
    public class TesteDeIntegracao : IClassFixture<ProducaoAPIWebApplicationFactory>
    {
        private readonly ProducaoAPIWebApplicationFactory app;
        public TesteDeIntegracao(ProducaoAPIWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task teste()
        {
            var client = app.CreateClient();

            var response = await client.GetAsync("/Maquina");

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        }
    }
}
