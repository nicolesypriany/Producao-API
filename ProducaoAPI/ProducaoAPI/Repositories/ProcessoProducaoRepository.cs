﻿using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Exceptions;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProcessoProducaoRepository : IProcessoProducaoRepository
    {
        private readonly ProducaoContext _context;
        public ProcessoProducaoRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas()
        {
            var producoes = await _context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Include(p => p.Maquina)
            .Include(p => p.Forma)
            .Include(p => p.Produto)
            .Where(m => m.Ativo == true)
            .OrderBy(p => p.Id)
            .ToListAsync();

            if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
            return producoes;
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes()
        {
            var producoes = await _context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Include(p => p.Maquina)
            .Include(p => p.Forma)
            .Include(p => p.Produto)
            .OrderBy(p => p.Id)
            .ToListAsync();

            if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção ativa encontrada.");
            return producoes;
        }

        public async Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id)
        {
            var producao = await _context.Producoes
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Include(p => p.Maquina)
            .Include(p => p.Forma)
            .Include(p => p.Produto)
            .Where(m => m.Ativo == true)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (producao == null) throw new NotFoundException("ID da produção não encontrado");
            return producao;
        }

        public async Task AdicionarAsync(ProcessoProducao producao)
        {
            await _context.Producoes.AddAsync(producao);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(ProcessoProducao producao)
        {
            _context.Producoes.Update(producao);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivasDetalhadas()
        {
            var producoes = await _context.Producoes
            .Include(p => p.Maquina)
            .Include(p => p.Forma)
            .ThenInclude(p => p.Produto)
            .Include(p => p.ProducaoMateriasPrimas)
            .ThenInclude(p => p.MateriaPrima)
            .Where(m => m.Ativo == true)
            .OrderBy(p => p.Id)
            .ToListAsync();

            if (producoes == null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
            return producoes;
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesPorProdutoEPeriodo(int produtoId, DateTime dataInicio, DateTime dataFim)
        {
            var producoes = await _context.Producoes
                .Where(p => p.ProdutoId == produtoId)
                .Where(p => p.Data >= dataInicio && p.Data <= dataFim)
                .Include(p => p.Maquina)
                .Include(p => p.Forma)
                .ThenInclude(p => p.Produto)
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .OrderBy(p => p.Data)
                .ToListAsync();

            if (producoes is null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
            return producoes;
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesPorMes(DateTime dataInicio, DateTime dataFim)
        {
            var producoes = await _context.Producoes
                .Where(p => p.Data >= dataInicio && p.Data <= dataFim)
                .Include(p => p.Maquina)
                .Include(p => p.Forma)
                .ThenInclude(p => p.Produto)
                .Include(p => p.ProducaoMateriasPrimas)
                .ThenInclude(p => p.MateriaPrima)
                .Where(m => m.Ativo == true)
                .OrderBy (p => p.Data)
                .ToListAsync();

            if (producoes is null || producoes.Count == 0) throw new NotFoundException("Nenhuma produção encontrada.");
            return producoes;
        }
    }
}
