namespace ArtVault.Utilizadores
{
    public class Padrao : Utilizador
    {
        public Padrao(int id, string email, string password, string username, string nome, string morada, int nif, int cc) 
                : base(id, email, password, username, nome, morada, nif, cc)
        {
        }

        public bool VerificarPassword(string password)
        {
            return this.password.Equals(password);
        }
    }
}
