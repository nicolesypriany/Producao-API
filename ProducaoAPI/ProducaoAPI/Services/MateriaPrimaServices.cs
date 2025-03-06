using ProducaoAPI.Models;
using ProducaoAPI.Repositories.Interfaces;
using ProducaoAPI.Requests;
using ProducaoAPI.Responses;
using ProducaoAPI.Services.Interfaces;
using ProducaoAPI.Validations;
using System.Xml;

namespace ProducaoAPI.Services
{
    public class MateriaPrimaServices : IMateriaPrimaService
    {
        private readonly IMateriaPrimaRepository _materiaPrimaRepository;

        public MateriaPrimaServices(IMateriaPrimaRepository materiaPrimaRepository)
        {
            _materiaPrimaRepository = materiaPrimaRepository;
        }

        public MateriaPrimaResponse EntityToResponse(MateriaPrima materiaPrima)
        {
            return new MateriaPrimaResponse(materiaPrima.Id, materiaPrima.Nome, materiaPrima.Fornecedor, materiaPrima.Unidade, materiaPrima.Preco, materiaPrima.Ativo);
        }

        public ICollection<MateriaPrimaResponse> EntityListToResponseList(IEnumerable<MateriaPrima> materiaPrima)
        {
            return materiaPrima.Select(m => EntityToResponse(m)).ToList();
        }

        public async Task<MateriaPrima> CriarMateriaPrimaPorXML(IFormFile arquivoXML)
        {
            var documentoXML = ConverterIFormFileParaXmlDocument(arquivoXML);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(documentoXML.NameTable);
            nsManager.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe");

            XmlNode? fornecedorNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:emit/ns:xNome", nsManager);
            if (fornecedorNode == null) throw new Exception("Erro ao ler arquivo XML: Fornecedor não encontrado.");
            string fornecedor = fornecedorNode.InnerText;

            XmlNode? produtoNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:xProd", nsManager);
            if (produtoNode == null) throw new Exception("Erro ao ler arquivo XML: Produto não encontrado.");
            string produto = produtoNode.InnerText;

            XmlNode? unidadeNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:uCom", nsManager);
            if (unidadeNode == null) throw new Exception("Erro ao ler arquivo XML: Unidade não encontrada.");
            string unidade = unidadeNode.InnerText == "T" ? "KG" : unidadeNode.InnerText;

            XmlNode? precoNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:vUnCom", nsManager);
            if (precoNode == null) throw new Exception("Erro ao ler arquivo XML: Preço não encontrado.");
            double preco = Convert.ToDouble(precoNode.InnerText.Replace(".", ","));
            if (unidade == "KG") preco /= 1000;

            MateriaPrimaRequest request = new MateriaPrimaRequest(produto, fornecedor, unidade, preco);
            await ValidarRequest(true, request);
            MateriaPrima materiaPrima = new MateriaPrima(request.Nome, request.Fornecedor, request.Unidade, request.Preco);
            await _materiaPrimaRepository.AdicionarAsync(materiaPrima);

            return materiaPrima;
        }

        public XmlDocument ConverterIFormFileParaXmlDocument(IFormFile arquivoXML)
        {
            XmlDocument doc = new XmlDocument();
            using (var stream = new MemoryStream())
            {
                arquivoXML.CopyTo(stream);
                stream.Position = 0;
                doc.Load(stream);
            }
            return doc;
        }

        public Task<IEnumerable<MateriaPrima>> ListarMateriasPrimasAtivas() => _materiaPrimaRepository.ListarMateriasPrimasAtivas();

        public Task<IEnumerable<MateriaPrima>> ListarTodasMateriasPrimas() => _materiaPrimaRepository.ListarTodasMateriasPrimas();

        public Task<MateriaPrima> BuscarMateriaPorIdAsync(int id) => _materiaPrimaRepository.BuscarMateriaPrimaPorIdAsync(id);

        public async Task<MateriaPrima> AdicionarAsync(MateriaPrimaRequest request)
        {
            await ValidarRequest(true, request);
            var materiaPrima = new MateriaPrima(request.Nome, request.Fornecedor, request.Unidade, request.Preco);
            await _materiaPrimaRepository.AdicionarAsync(materiaPrima);
            return materiaPrima;
        }

        public async Task<MateriaPrima> AtualizarAsync(int id, MateriaPrimaRequest request)
        {
            var materiaPrima = await _materiaPrimaRepository.BuscarMateriaPrimaPorIdAsync(id);
            await ValidarRequest(false, request, materiaPrima.Nome);

            materiaPrima.Nome = request.Nome;
            materiaPrima.Fornecedor = request.Fornecedor;
            materiaPrima.Unidade = request.Unidade;
            materiaPrima.Preco = request.Preco;

            await _materiaPrimaRepository.AtualizarAsync(materiaPrima);
            return materiaPrima;
        }

        public async Task<MateriaPrima> InativarMateriaPrima(int id)
        {
            var materiaPrima = await BuscarMateriaPorIdAsync(id);
            materiaPrima.Ativo = false;
            await _materiaPrimaRepository.AtualizarAsync(materiaPrima);
            return materiaPrima;
        }

        private async Task ValidarRequest(bool Cadastrar, MateriaPrimaRequest request, string nomeAtual = "")
        {
            var nomeMateriasPrimas = await _materiaPrimaRepository.ListarNomes();

            ValidarCampos.Nome(Cadastrar, nomeMateriasPrimas, request.Nome, nomeAtual);
            ValidarCampos.String(request.Nome, "Nome");
            ValidarCampos.String(request.Fornecedor, "Fornecedor");
            ValidarCampos.String(request.Unidade, "Unidade");
            ValidarCampos.Unidade(request.Unidade);
            ValidarCampos.Preco(request.Preco);
        }
    }
}
