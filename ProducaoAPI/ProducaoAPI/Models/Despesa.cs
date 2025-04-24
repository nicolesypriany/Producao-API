namespace ProducaoAPI.Models
{
    public class Despesa
    {
        public Despesa(string nome, string descricao, decimal valor)
        {
            Nome = nome;
            Descricao = descricao;
            Valor = valor;
            Ativo = true;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool Ativo { get; set; }
    }
}
