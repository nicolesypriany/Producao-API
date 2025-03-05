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

        public static void NomeParaCadastrarObjeto(IEnumerable<string> nomes, string nome)
        {
            if (nomes.Contains(nome)) throw new BadRequestException("Já existe um cadastro com este nome!");
        }

        public static void NomeParaAtualizarObjeto(IEnumerable<string> nomes, string nome, string novoNome)
        {
            if (nomes.Contains(nome) && nome != novoNome) throw new BadRequestException("Já existe um cadastro com este nome!");
        }
    }
}
