using ArtVault.Business.Utilizadores;

namespace ArtVault.Business
{
    public class IArtVaultFacade
    {
        private Utilizador user_atual;
        private Leilao leilao_atual;

        public void SetUserAtual(Utilizador user)
        {
            user_atual = user;
        }

        public void SetLeilaoAtual(Leilao leilao)
        {
            leilao_atual = leilao;
        }

        // return 0 se credenciais inválidas
        // return 1 se sucesso Padrao
        // return 2 se sucesso Artista
        // return 3 se sucesso Admin
        /*
        public int Login(string email, string password)
        {
            Utilizador u = GetUserByEmail(email);
            if (u != null && u.ValidPassword(password)) 
            {
                SetUserAtual(u);
                if (u is Padrao) { return 1; }
                if (u is Artista) { return 2; }
                if (u is Admin) { return 3; }
            }
            return 0;
        } */

    }
}
