//using ProducaoAPI.Data;
//using Xunit.Abstractions;

//namespace ProducaoAPI.Test.FormaRepositorya
//{
//    [Collection(nameof(ContextoCollection))]

//    public class FormaListar
//    {
//        private readonly ProducaoContext context;

//        public FormaListar(ITestOutputHelper output, ContextoFixture fixture)
//        {
//            context = fixture.Context;
//            output.WriteLine(context.GetHashCode().ToString());
//        }

//        [Fact]
//        public async void RetornaNuloQuandoIdInexistente()
//        {
//            //arrange
//            //var formaRepository = new Repositories.FormaRepository(context);

//            //act & assert
//            //var exception = await Assert.ThrowsAsync<Exception>(() => formaRepository.ListarFormasAsync());
//            //Assert.Equal("ID da forma não encontrado!", exception.Message);
//        }

//        [Fact]
//        public async void RetornaIDQuandoIdExistente()
//        {
//            //arrange
//            //var formaRepository = new Repositories.FormaRepository(context);

//            //act & assert
//            //var formaRecuperada = await formaRepository.BuscarFormaPorIdAsync(1);
//            //Assert.NotNull(formaRecuperada);
//            //Assert.Equal(1, formaRecuperada.Id);
//        }
//    }
//}
