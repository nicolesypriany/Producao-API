namespace ProducaoAPI.Requests;

public record LogRequest(
    string Acao,
    string Objeto,
    int ObjetoId
);
