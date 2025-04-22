using ProducaoAPI.Exceptions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using System.Text.Json;

namespace ProducaoAPI.Services
{
    public class FreteServices : IFreteService
    {
        private readonly string _apiKey = "5b3ce3597851110001cf62487d4a448205d9405ebbc1df1ea0b5defe";
        private readonly HttpClient _client;
        private readonly IProdutoService _produtoService;

        public FreteServices(IProdutoService produtoService)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.openrouteservice.org/")
            };
            _produtoService = produtoService;
        }

        public async Task<FreteResponse> CalcularPreco(FreteRequest request)
        {
            var coordenadasOrigem = await BuscarCoordenadas(request.EnderecoOrigem);
            var coordenadasDestino = await BuscarCoordenadas(request.EnderecoDestino);
            var distanciaEmQuilometros = await RetornaDistanciaDaRota([coordenadasOrigem, coordenadasDestino]);
            var precoViagem = (distanciaEmQuilometros / request.KmPorLitro) * request.PrecoDiesel;
            var numeroDePaletes = request.QuantidadeProduto / request.QuantidadePorPalete;
            int numeroDeViagens = (int)Math.Ceiling((double)numeroDePaletes / request.PaletesPorCarga);
            var precoTotal = precoViagem * numeroDeViagens;

            return new FreteResponse(
                distanciaEmQuilometros, 
                numeroDeViagens, 
                request.PrecoDiesel, 
                Math.Round(precoTotal)
            );
        }

        private async Task<CoordinatesResponse> BuscarCoordenadas(string endereco)
        {
            var url = $"/geocode/search?api_key={_apiKey}&text={endereco}";

            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var documento = JsonDocument.Parse(json);
                var feature = documento.RootElement.GetProperty("features")[0];
                var coordenadas = feature.GetProperty("geometry").GetProperty("coordinates");

                var longitude = coordenadas[0].GetDouble();
                var latitude = coordenadas[1].GetDouble();
                return new CoordinatesResponse(longitude, latitude);
            }
            else
            {
                throw new NotFoundException("Nenhuma coordenada encontrada para o endereço informado.");
            }
        }

        private async Task<double> RetornaDistanciaDaRota(List<CoordinatesResponse> coordinates)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "v2/directions/driving-car/geojson");
            request.Headers.Add("Authorization", _apiKey);
            request.Content = new StringContent(
            JsonSerializer.Serialize(new 
            {
                coordinates = new List<List<double>> 
                {
                    new List<double> { coordinates[0].Longitude, coordinates[0].Latitude },
                    new List<double> { coordinates[1].Longitude, coordinates[1].Latitude }
                }
            }),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            Console.WriteLine(request.Content);

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var distanceMeters = doc
                .RootElement
                .GetProperty("features")[0]
                .GetProperty("properties")
                .GetProperty("segments")[0]
                .GetProperty("distance")
                .GetDouble();

            return distanceMeters / 1000;
        }
    }
}
