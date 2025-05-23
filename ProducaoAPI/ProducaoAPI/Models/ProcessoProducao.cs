﻿namespace ProducaoAPI.Models
{
    public class ProcessoProducao
    {
        public ProcessoProducao(DateTime data, int maquinaId, int formaId, int produtoId, int ciclos)
        {
            Data = data;
            MaquinaId = maquinaId;
            FormaId = formaId;
            ProdutoId = produtoId;
            Ciclos = ciclos;
            Ativo = true;
            DataCriacao = DateTime.Now;
            DataEdicao = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int MaquinaId { get; set; }
        public Maquina Maquina { get; set; }
        public int FormaId { get; set; }
        public Forma Forma { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int Ciclos { get; set; }
        public decimal? QuantidadeProduzida { get; set; }
        public ICollection<ProcessoProducaoMateriaPrima> ProducaoMateriasPrimas { get; set; }
        public decimal? CustoUnitario { get; set; }
        public decimal? CustoTotal { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
        public bool Ativo { get; set; }
    }
}
