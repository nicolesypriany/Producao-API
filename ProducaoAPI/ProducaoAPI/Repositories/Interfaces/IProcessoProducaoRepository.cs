﻿using ProducaoAPI.Models;

namespace ProducaoAPI.Repositories.Interfaces
{
    public interface IProcessoProducaoRepository
    {
        Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas();
        Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes();
        Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id);
        Task AdicionarAsync(ProcessoProducao producao);
        Task AtualizarAsync(ProcessoProducao producao);
        Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivasDetalhadas();
        Task<IEnumerable<ProcessoProducao>> ListarProducoesPorProdutoEPeriodo(int produtoId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<ProcessoProducao>> ListarProducoesPorMes(DateTime dataInicio, DateTime dataFim);
    }
}
