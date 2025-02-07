namespace ProducaoAPI.Models
{
    public class Produto
    {
        public Produto(string nome, string medidas, string unidade, int pecasPorUnidade)
        {
            Nome = nome;
            Medidas = medidas;
            Unidade = unidade;
            PecasPorUnidade = pecasPorUnidade;
            Ativo = true;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Medidas { get; set; }
        public string Unidade { get; set; }
        public int PecasPorUnidade { get; set; }
        public ICollection<Forma> Formas { get; set; }
        public ICollection<ProcessoProducao> Producoes { get; set; }
        public bool Ativo { get; set; }
    }
}
