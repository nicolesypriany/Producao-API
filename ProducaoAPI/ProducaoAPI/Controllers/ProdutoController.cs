﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services;

namespace ProducaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : Controller
    {
        private readonly ProducaoContext _context;
        public ProdutoController(ProducaoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponse>>> ListarProdutos()
        {
            var produtos = await _context.Produtos.Where(m => m.Ativo == true).ToListAsync();
            if (produtos == null) return NotFound();
            return Ok(ProdutoServices.EntityListToResponseList(produtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponse>> BuscarProdutoPorId(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            return Ok(ProdutoServices.EntityToResponse(produto));
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponse>> CadastrarProduto(ProdutoRequest req)
        {
            var produto = new Produto(req.Nome, req.Medidas, req.Unidade, req.PecasPorUnidade);
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoResponse>> AtualizarProduto(int id, ProdutoRequest req)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            produto.Nome = req.Nome;
            produto.Medidas = req.Medidas;
            produto.Unidade = req.Unidade;
            produto.PecasPorUnidade = req.PecasPorUnidade;

            await _context.SaveChangesAsync();
            return Ok(produto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ProdutoResponse>> InativarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();
            produto.Ativo = false;

            await _context.SaveChangesAsync();
            return Ok(produto);
        }
    }
}
