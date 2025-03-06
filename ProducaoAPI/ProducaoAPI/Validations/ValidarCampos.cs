using ProducaoAPI.Exceptions;

namespace ProducaoAPI.Validations
{
    public static class ValidarCampos
    {
        public static void String(string campo, string nomeCampo)
        {
            if (string.IsNullOrEmpty(campo)) throw new BadRequestException($"O campo '{nomeCampo}' não pode estar vazio.");
        }

        public static void Inteiro(int campo, string nomeCampo)
        {
            if (campo < 1) throw new BadRequestException($"O número de '{nomeCampo}' deve ser maior do que 0.");
        }

        public static void Double(double campo, string nomeCampo)
        {
            if (campo < 1) throw new BadRequestException($"O valor de '{nomeCampo}' deve ser maior do que 0.");
        }

        public static void Unidade(string unidade)
        {
            if (unidade.Length > 5) throw new BadRequestException("A sigla da unidade não pode ter mais de 5 caracteres.");
        }

        public static void Preco(double preco)
        {
            if (preco <= 0) throw new BadRequestException("O preço não pode ser igual ou menor que 0.");
        }

        public static void Nome(bool Cadastrar, IEnumerable<string> nomes, string nomeNovo, string nomeAtual = "")
        {
            if (Cadastrar)
            {
                if (nomes.Contains(nomeNovo)) throw new BadRequestException("Já existe um cadastro com este nome!");
            }
            else
            {
                if (nomes.Contains(nomeNovo) && nomeAtual != nomeNovo) throw new BadRequestException("Já existe um cadastro com este nome!");
            }
        }
    }
}
