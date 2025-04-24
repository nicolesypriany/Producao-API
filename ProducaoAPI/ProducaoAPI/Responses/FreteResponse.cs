namespace ProducaoAPI.Responses;

public record FreteResponse(CoordinatesResponse CoordenadasOrigem, CoordinatesResponse CoordenadasDestino, double DistanciaEmQuilometros, int NumeroDeViagens, double PrecoLitro, double PrecoTotal, string GeoJson);