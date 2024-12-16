using ProducaoAPI.Data;
using ProducaoAPI.Models;
using ProducaoAPI.Responses;
using System.Xml;

namespace ProducaoAPI.Services
{
    public static class MateriaPrimaServices
    {
        public static MateriaPrimaResponse EntityToResponse(MateriaPrima materiaPrima)
        {
            return new MateriaPrimaResponse(materiaPrima.Id, materiaPrima.Nome, materiaPrima.Fornecedor, materiaPrima.Unidade, materiaPrima.Preco, materiaPrima.Ativo);
        }
        public static ICollection<MateriaPrimaResponse> EntityListToResponseList(IEnumerable<MateriaPrima> materiaPrima)
        {
            return materiaPrima.Select(m => EntityToResponse(m)).ToList();
        }
        public static MateriaPrima CriarMateriaPrimaPorXML(ProducaoContext context, IFormFile arquivoXML)
        {
            var documentoXML = SalvarXML(arquivoXML);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(documentoXML.NameTable);
            nsManager.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe");

            XmlNode? fornecedorNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:emit/ns:xNome", nsManager);
            string fornecedor = fornecedorNode.InnerText;

            XmlNode? produtoNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:xProd", nsManager);
            string produto = produtoNode.InnerText;

            XmlNode? unidadeNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:uCom", nsManager);
            string unidade = unidadeNode.InnerText == "T" ? "KG" : unidadeNode.InnerText;

            XmlNode? precoNode = documentoXML.SelectSingleNode("//ns:nfeProc/ns:NFe/ns:infNFe/ns:det/ns:prod/ns:vUnCom", nsManager);
            double preco = Convert.ToDouble(precoNode.InnerText.Replace(".", ","));
            if (unidade == "KG") preco /= 1000;

            MateriaPrima materiaPrima = new MateriaPrima(produto, fornecedor, unidade, preco);
            context.MateriasPrimas.Add(materiaPrima);
            context.SaveChanges();

            var filePatch = Path.Combine("Storage", arquivoXML.FileName);
            File.Delete(filePatch);

            return materiaPrima;
        }

        private static XmlDocument SalvarXML(IFormFile arquivoXML)
        {
            var filePatch = Path.Combine("Storage", arquivoXML.FileName);
            using Stream fileStream = new FileStream(filePatch, FileMode.Create);
            arquivoXML.CopyTo(fileStream);
            fileStream.Close();
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(filePatch));
            return doc;
        }
    }
}
