namespace ArtVault.Business.Utilizadores
{
    public class Admin : Utilizador
    {
        public Admin(int id, string email, string password, string username, string nome, string morada, int nif, int cc)
                : base(id, email, password, username, nome, morada, nif, cc)
        {
        }
    }
}
