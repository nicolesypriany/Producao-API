using ProducaoAPI.Exceptions;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using System.Text.Json;

namespace ProducaoAPI.Services
{
    public class RouteServices
    {
        private readonly string _apiKey = "5b3ce3597851110001cf62487d4a448205d9405ebbc1df1ea0b5defe";
        private readonly HttpClient _client;

        public RouteServices()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.openrouteservice.org/")
            };
        }

        public async Task<RouteResponse> CalcularPreco(FreteRequest enderecos)
        {
            var coordenadas = await BuscarCoordenadas(enderecos);
            var distancia = await RetornaDistanciaDaRota(coordenadas);
            var litros = Convert.ToDecimal(distancia) / enderecos.KmPorLitro;
            var precoTotal = litros * enderecos.PrecoDiesel;
            return new RouteResponse(distancia, enderecos.PrecoDiesel, precoTotal);
        }

        public async Task<List<CoordinatesResponse>> BuscarCoordenadas(FreteRequest endereco)
        {
            var url = $"/geocode/search?api_key={_apiKey}&text={endereco.EnderecoOrigem}";

            var listaCoordenadas = new List<CoordinatesResponse>();
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var documento = JsonDocument.Parse(json);
                var feature = documento.RootElement.GetProperty("features")[0];
                var coordenadas = feature.GetProperty("geometry").GetProperty("coordinates");

                var longitude = coordenadas[0].GetDouble();
                var latitude = coordenadas[1].GetDouble();

                listaCoordenadas.Add(new CoordinatesResponse(longitude, latitude));
            }

            url = $"/geocode/search?api_key={_apiKey}&text={endereco.EnderecoDestino}";
 
            response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                using var documento = JsonDocument.Parse(json);
                var feature = documento.RootElement.GetProperty("features")[0];
                var coordenadas = feature.GetProperty("geometry").GetProperty("coordinates");

                var longitude = coordenadas[0].GetDouble();
                var latitude = coordenadas[1].GetDouble();

                listaCoordenadas.Add(new CoordinatesResponse(longitude, latitude));
            }
            else
            {
                throw new NotFoundException("Nenhuma coordenada encontrada para o endereço informado.");
            }

            return listaCoordenadas;
        }

        public async Task<double> RetornaDistanciaDaRota(List<CoordinatesResponse> coordinates)
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
