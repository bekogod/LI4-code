namespace ArtVault.Business.Utilizadores
{
    public class Artista : Utilizador
    {
        private bool ativo;

        public Artista(int id, string email, string password, string username, string nome, string morada, int nif, int cc)
                : base(id, email, password, username, nome, morada, nif, cc)
        {
            ativo = false;
        }

        public void AtivarConta()
        {
            ativo = true;
        }

        public override int UserType() { return 2; }
    }
}
