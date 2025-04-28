using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;
using System.Text;

namespace ProducaoAPI.Services
{
    public class ProcessoProducaoServices : IProcessoProducaoService
    {
        private readonly IProcessoProducaoRepository _producaoRepository;
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;
        private readonly IFormaRepository _formaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProducaoMateriaPrimaRepository _producaoMateriaPrimaRepository;
        private readonly IProducaoMateriaPrimaService _producaoMateriaPrimaService;
        private readonly IMaquinaRepository _maquinaRepository;

        public ProcessoProducaoServices(IProcessoProducaoRepository producaoRepository, IMateriaPrimaRepository materiaPrimaRepository, IFormaRepository formaRepository, IProdutoRepository produtoRepository, IProducaoMateriaPrimaRepository producaoMateriaPrimaRepository, IProducaoMateriaPrimaService producaoMateriaPrimaService, IMaquinaRepository maquinaRepository)
        {
            _producaoRepository = producaoRepository;
            _materiaPrimaRepository = materiaPrimaRepository;
            _formaRepository = formaRepository;
            _produtoRepository = produtoRepository;
            _producaoMateriaPrimaRepository = producaoMateriaPrimaRepository;
            _producaoMateriaPrimaService = producaoMateriaPrimaService;
            _maquinaRepository = maquinaRepository;
        }

        public async Task<ProcessoProducaoResponse> EntityToResponse(ProcessoProducao producao)
        {
            var producoesMateriasPrimas = await _producaoMateriaPrimaService.EntityListToResponseList(producao.ProducaoMateriasPrimas);
            return new ProcessoProducaoResponse(
                producao.Id,
                producao.Data,
                producao.Maquina.Nome,
                producao.Forma.Nome,
                producao.Produto.Nome,
                producao.Ciclos,
                producoesMateriasPrimas,
                producao.QuantidadeProduzida,
                producao.Produto.Unidade,
                producao.CustoUnitario,
                producao.CustoTotal,
                producao.DataCriacao,
                producao.DataEdicao,
                producao.Ativo
            );
        }

        public async Task<ICollection<ProcessoProducaoResponse>> EntityListToResponseList(IEnumerable<ProcessoProducao> producoes)
        {
            var responseList = new List<ProcessoProducaoResponse>();
            foreach (var producao in producoes)
            {
                var response = await EntityToResponse(producao);
                responseList.Add(response);
            }
            return responseList;
        }

        public async Task<List<ProcessoProducaoMateriaPrima>> CriarProducoesMateriasPrimas(ICollection<ProcessoProducaoMateriaPrimaRequest> materiasPrimas, int ProducaoId)
        {
            var producoesMateriasPrimas = new List<ProcessoProducaoMateriaPrima>();

            foreach (var materiaPrima in materiasPrimas)
            {
                var materiaPrimaSelecionada = await _materiaPrimaRepository.BuscarMateriaPrimaPorIdAsync(materiaPrima.Id);
                var producaoMateriaPrima = new ProcessoProducaoMateriaPrima(ProducaoId, materiaPrimaSelecionada.Id, materiaPrimaSelecionada.Preco, materiaPrima.Quantidade);
                producoesMateriasPrimas.Add(producaoMateriaPrima);
            }
            return producoesMateriasPrimas;
        }

        public async Task CalcularProducao(int producaoId)
        {
            var producao = await _producaoRepository.BuscarProducaoPorIdAsync(producaoId);
            var forma = await _formaRepository.BuscarFormaPorIdAsync(producao.FormaId);
            var produto = await _produtoRepository.BuscarProdutoPorIdAsync(producao.ProdutoId);
            decimal quantidadeProduzida = (producao.Ciclos * forma.PecasPorCiclo) / produto.PecasPorUnidade;

            decimal custoTotal = 0;
            foreach (var producaoMateriaPrima in producao.ProducaoMateriasPrimas)
            {
                custoTotal += producaoMateriaPrima.Quantidade * producaoMateriaPrima.Preco;
            }

            producao.QuantidadeProduzida = quantidadeProduzida;
            producao.CustoTotal = custoTotal;
            producao.CustoUnitario = custoTotal / quantidadeProduzida;
            await _producaoRepository.AtualizarAsync(producao);
        }

        public Task<IEnumerable<ProcessoProducao>> ListarProducoesAtivas() => _producaoRepository.ListarProducoesAtivas();
        public Task<IEnumerable<ProcessoProducao>> ListarTodasProducoes() => _producaoRepository.ListarTodasProducoes();

        public Task<ProcessoProducao> BuscarProducaoPorIdAsync(int id) => _producaoRepository.BuscarProducaoPorIdAsync(id);

        public async Task<ProcessoProducao> AdicionarAsync(ProcessoProducaoRequest request)
        {
            await ValidarRequest(request);
            var forma = await _formaRepository.BuscarFormaPorIdAsync(request.FormaId);
            var producao = new ProcessoProducao(request.Data, request.MaquinaId, request.FormaId, forma.ProdutoId, request.Ciclos);
            await _producaoRepository.AdicionarAsync(producao);

            var producaoMateriasPrimas = await CriarProducoesMateriasPrimas(request.MateriasPrimas, producao.Id);
            foreach (var producaMateriaPrima in producaoMateriasPrimas)
            {
                await _producaoMateriaPrimaRepository.AdicionarAsync(producaMateriaPrima);
            }

            await CalcularProducao(producao.Id);
            return producao;
        }

        public async Task<ProcessoProducao> AtualizarAsync(int id, ProcessoProducaoRequest request)
        {
            await ValidarRequest(request);
            var forma = await _formaRepository.BuscarFormaPorIdAsync(request.FormaId);
            var producao = await BuscarProducaoPorIdAsync(id);
            await _producaoMateriaPrimaService.VerificarProducoesMateriasPrimasExistentes(id, request.MateriasPrimas);

            producao.Data = request.Data;
            producao.MaquinaId = request.MaquinaId;
            producao.FormaId = request.FormaId;
            producao.ProdutoId = forma.ProdutoId;
            producao.Ciclos = request.Ciclos;

            await CalcularProducao(id);
            producao.DataEdicao = DateTime.Now;
            await _producaoRepository.AtualizarAsync(producao);
            return producao;
        }

        public async Task<ProcessoProducao> InativarProducao(int id)
        {
            var producao = await BuscarProducaoPorIdAsync(id);
            producao.Ativo = false;
            await _producaoRepository.AtualizarAsync(producao);
            return producao;
        }

        public Task<Forma> BuscarFormaPorIdAsync(int id) => _formaRepository.BuscarFormaPorIdAsync(id);

        public Task AdicionarProducaoMateriaAsync(ProcessoProducaoMateriaPrima producaoMateriaPrima) => _producaoMateriaPrimaRepository.AdicionarAsync(producaoMateriaPrima);

        public async Task<FileStreamResult> GerarRelatorioTXT()
        {
            var producoes = await _producaoRepository.ListarProducoesAtivasDetalhadas();
            string textos = "";

            foreach (var producao in producoes)
            {
                textos += ("ID: " + producao.Id + " Data: " + producao.Data + " Maquina: " + producao.Maquina.Nome + " Forma: " + producao.Forma.Nome + " Produto: " + producao.Produto.Nome + " Quantidade Produzida: " + producao.QuantidadeProduzida + " " + producao.Produto.Unidade + "\n").ToString();
            }

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(textos));
            return new FileStreamResult(stream, "text/plain")
            {
                FileDownloadName = "relatorio-producao.txt"
            };
        }

        public async Task<FileStreamResult> GerarRelatorioXLSX()
        {
            var producoes = await _producaoRepository.ListarProducoesAtivasDetalhadas();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Producoes");

                worksheet.Cells[1, 1].Value = "Data";
                worksheet.Cells[1, 2].Value = "Máquina";
                worksheet.Cells[1, 3].Value = "Produto";
                worksheet.Cells[1, 4].Value = "Ciclos";
                worksheet.Cells[1, 5].Value = "Quantidade Produzida";
                worksheet.Cells[1, 6].Value = "Unidade";
                worksheet.Cells[1, 7].Value = "Custo Total";
                worksheet.Cells[1, 8].Value = "Custo Unitário";

                var materiasPrimas = await _materiaPrimaRepository.ListarMateriasPrimasAtivas();

                var i = 9;
                foreach(var materia in materiasPrimas)
                {
                    worksheet.Cells[1, i].Value = materia.Nome;
                    worksheet.Cells[1, i+1].Value = "Unidade";
                    worksheet.Cells[1, i+2].Value = "Preço";
                    worksheet.Cells[1, i+3].Value = "Total";
                    i+=4;
                }

                var row = 2;
                foreach (var producao in producoes)
                {
                    worksheet.Cells[row, 1].Style.Numberformat.Format = "dd/mm/yyyy";
                    worksheet.Cells[row, 1].Value = producao.Data;
                    worksheet.Cells[row, 2].Value = producao.Maquina.Nome;
                    worksheet.Cells[row, 3].Value = producao.Produto.Nome;
                    worksheet.Cells[row, 4].Value = producao.Ciclos;
                    worksheet.Cells[row, 5].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[row, 5].Value = producao.QuantidadeProduzida;
                    worksheet.Cells[row, 6].Value = producao.Produto.Unidade;
                    worksheet.Cells[row, 7].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[row, 7].Value = producao.CustoTotal;
                    worksheet.Cells[row, 8].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[row, 8].Value = producao.CustoUnitario;
                    
                    i = 9;
                    foreach (var materia in producao.ProducaoMateriasPrimas)
                    {
                        if(materia.MateriaPrima.Nome == worksheet.Cells[1, i].Value.ToString())
                        {
                            worksheet.Cells[row, i].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[row, i].Value = materia.Quantidade;
                            worksheet.Cells[row, i + 1].Value = materia.MateriaPrima.Unidade;
                            worksheet.Cells[row, i + 2].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[row, i + 2].Value = materia.MateriaPrima.Preco;
                            worksheet.Cells[row, i + 3].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[row, i + 3].Value = materia.Quantidade * materia.MateriaPrima.Preco;
                            i+=4;
                        }
                    }
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "relatorio-producao.xlsx"
                };
            }
        }

        private async Task ValidarRequest(ProcessoProducaoRequest request)
        {
            ValidarCampos.Inteiro(request.Ciclos, "Ciclos");
            await ValidarMaquina(request.MaquinaId);
            await ValidarForma(request.FormaId);

            foreach (var materiaPrima in request.MateriasPrimas)
            {
                await ValidarMateriaPrima(materiaPrima.Id);
                ValidarCampos.Decimal(materiaPrima.Quantidade, "Quantidade de Matéria-Prima");
            }
        }

        private async Task ValidarMaquina(int id)
        {
            await _maquinaRepository.BuscarMaquinaPorIdAsync(id);
        }

        private async Task ValidarForma(int id)
        {
            await _formaRepository.BuscarFormaPorIdAsync(id);
        }

        private async Task ValidarMateriaPrima(int id)
        {
            await _materiaPrimaRepository.BuscarMateriaPrimaPorIdAsync(id);
        }

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesPorProdutoEPeriodo(int produtoId, DateTime dataInicio, DateTime dataFim) => await _producaoRepository.ListarProducoesPorProdutoEPeriodo(produtoId, dataInicio, dataFim);

        public async Task<IEnumerable<ProcessoProducao>> ListarProducoesPorMes(DateTime dataInicio, DateTime dataFim) => await _producaoRepository.ListarProducoesPorMes(dataInicio, dataFim);
    }
}
