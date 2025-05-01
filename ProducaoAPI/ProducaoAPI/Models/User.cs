using ProducaoAPI.Enums;

namespace ProducaoAPI.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cargo { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }

        public User(string nome, string email)
        {
            Nome = nome;
            Email = email;
            Cargo = Cargos.Consultor.ToString();
            Console.WriteLine(Cargos.Consultor.ToString());
        }

        public void AlterarSenha(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}
