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

        /*
        public bool CheckCredentials(string email, string password)
        {
            Utilizador u = GetUserByEmail(email);
            if (u != null && u.ValidPassword(password)) 
            {
                SetUserAtual(u);
                return true;
            }
            return false;
        } */

        public int GetUserType()
        {
            return user_atual.UserType();
        }



    }
}
