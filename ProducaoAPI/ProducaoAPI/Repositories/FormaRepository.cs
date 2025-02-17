﻿using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class FormaRepository : IFormaRepository
    {
        private readonly ProducaoContext _context;
        private readonly IProdutoRepository _produtoRepository;
        public FormaRepository(ProducaoContext context, IProdutoRepository produtoRepository)
        {
            _context = context;
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<Forma>> ListarFormasAtivas()
        {
            try
            {
                var formas = await _context.Formas
                    .Where(m => m.Ativo == true)
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NullReferenceException("Nenhuma forma ativa encontrada.");
                return formas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Forma>> ListarTodasFormas()
        {
            try
            {
                var formas = await _context.Formas
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .ToListAsync();

                if (formas == null || formas.Count == 0) throw new NullReferenceException("Nenhuma forma encontrada.");
                return formas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Forma> BuscarFormaPorIdAsync(int id)
        {
            try
            {
                var forma = await _context.Formas
                    .Include(f => f.Maquinas)
                    .Include(f => f.Produto)
                    .FirstOrDefaultAsync(f => f.Id == id);

                if (forma is null) throw new HttpStatusCodeException(404, "ID da forma não encontrado.");
                return forma;
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AdicionarAsync(Forma forma)
        {
            try
            {
                await _context.Formas.AddAsync(forma);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AtualizarAsync(Forma forma)
        {
            try
            {
                _context.Formas.Update(forma);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ValidarDados(Forma forma)
        {
            if (string.IsNullOrWhiteSpace(forma.Nome)) throw new ArgumentException("O campo \"Nome\" não pode estar vazio.");
            if (forma.PecasPorCiclo < 1) throw new ArgumentException("O número de peças por ciclo deve ser maior do que 0.");

            var produtos = await _produtoRepository.ListarProdutosAtivos();
            if (!produtos.Contains(forma.Produto)) throw new NullReferenceException("ID do produto não encontrado.");
        }
    }
}