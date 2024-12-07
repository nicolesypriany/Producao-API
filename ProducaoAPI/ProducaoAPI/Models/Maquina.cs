namespace ProducaoAPI.Models
{
    public class Maquina
    {
        public Maquina(string nome, string marca)
        {
            Nome = nome;
            Marca = marca;
            Ativo = true;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public List<Forma> Formas { get; set; }
        public ICollection<ProcessoProducao> Producoes { get; set; }
        public bool Ativo { get; set; }
    }
}
