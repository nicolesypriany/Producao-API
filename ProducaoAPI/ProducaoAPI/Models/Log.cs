namespace ProducaoAPI.Models
{
    public class Log
    {
        public Log(string acao, string objeto, int idObjeto, int userId)
        {
            Acao = acao;
            Objeto = objeto;
            IdObjeto = idObjeto;
            UserId = userId;
            Data = DateTime.Now;
        }

        public Log(string acao, string objeto, int idObjeto, int userId, string dadoAlterado, string conteudo)
        {
            Acao = acao;
            Objeto = objeto;
            IdObjeto = idObjeto;
            UserId = userId;
            Data = DateTime.Now;
            DadoAlterado = dadoAlterado;
            Conteudo = conteudo;
        }

        public int Id { get; set; }
        public string Acao { get; set; }
        public string Objeto { get; set; }
        public int IdObjeto { get; set; }
        public int UserId { get; set; }
        public DateTime Data { get; set; }
        public string DadoAlterado { get; set; }
        public string Conteudo { get; set; }
    }
}
