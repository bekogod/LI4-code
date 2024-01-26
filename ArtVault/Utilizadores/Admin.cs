namespace ArtVault.Utilizadores
{
    public class Admin : Utilizador
    {
        public Admin(int id, string email, string password, string username, string nome, string morada, string nif, string cc) 
                : base(id, email, password, username, nome, morada, nif, cc)
        {
        }
    }
}
