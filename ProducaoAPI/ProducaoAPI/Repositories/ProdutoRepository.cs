using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;

namespace ProducaoAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProducaoContext _context;
        string connectionString = "Host=localhost;Port=5432;Database=producao-api;Username=postgres;Password=admin";
        
        public ProdutoRepository(ProducaoContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.ExecuteAsync("INSERT INTO produtos (nome, medidas, unidade, pecas_por_unidade, ativo) VALUES(@nome, @medidas, @unidade, @pecasPorUnidade, @ativo)",
                    new
                    {
                        nome = produto.Nome,
                        medidas = produto.Medidas,
                        unidade = produto.Unidade,
                        pecasPorUnidade = produto.PecasPorUnidade,
                        ativo = true
                    });
            }
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            var produto = await connection.QueryFirstOrDefaultAsync<Produto>("SELECT id, nome, medidas, unidade, pecas_por_unidade, ativo FROM produtos WHERE id = @idSelecionado",
                new { idSelecionado = id });

            return produto;
            //return await _context.Produtos.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Produto>> ListarProdutosAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
           
            var produtos = await connection.QueryAsync<Produto>("SELECT id, nome, medidas, unidade, pecas_por_unidade, ativo FROM produtos");

            return produtos;

            //return await _context.Produtos.Where(m => m.Ativo == true).ToListAsync();
        }
    }
}
