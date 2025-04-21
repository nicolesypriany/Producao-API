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
        public async Task<CoordinatesResponse> BuscarCoordenadas(string endereco)
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

        public async Task<double> RetornaDistanciaDaRota(RouteRequest bodyRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "v2/directions/driving-car/geojson");
            request.Headers.Add("Authorization", _apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(new { coordinates = bodyRequest }),
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

            return distanceMeters;
        }
    }
}
