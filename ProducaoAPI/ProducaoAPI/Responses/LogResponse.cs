namespace ProducaoAPI.Responses;

public record LogResponse(
    int Id, 
    string Acao,
    string Objeto,
    int IdObjeto,
    string Usuario,
    DateTime Data,
    string? DadoAlterado,
    string? Conteudo
);
