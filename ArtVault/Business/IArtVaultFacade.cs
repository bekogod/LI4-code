using ArtVault.Business.Utilizadores;
using ArtVault.DAOs;
namespace ArtVault.Business
{
    public class IArtVaultFacade
    {
        private Utilizador user_atual;
        private Leilao leilao_atual;
        private IDatabaseFacade IDBFacade = new IDatabaseFacade();

        public IArtVaultFacade()
        {
            user_atual = null;
        }

        public void SetUserAtual(Utilizador user)
        {
            user_atual = user;
        }
        public Utilizador GetUserAtual()
        {
            return user_atual;
        }
        public string GetUtilizadorAtualName()
        {
            return user_atual.GetNome();
        }


        public void SetLeilaoAtual(Leilao leilao)
        {
            leilao_atual = leilao;
        }

        
        public bool CheckCredentials(string email, string password)
        {
            string user = IDBFacade.GetUserByEmail(email);
            if (user != null)
            {
                string[] sUser = user.Split(';');
                if (sUser.Length == 10)
                {
                    string type = sUser[8];
                    Utilizador u;
                    if (type == "1")
                    {
                        u = new Padrao(int.Parse(sUser[0]), sUser[3], sUser[2], sUser[1], sUser[4], sUser[5], int.Parse(sUser[6]), int.Parse(sUser[7]));
                    }
                    else if (type == "2")
                    {
                        u = new Artista(int.Parse(sUser[0]), sUser[3], sUser[2], sUser[1], sUser[4], sUser[5], int.Parse(sUser[6]), int.Parse(sUser[7]), int.Parse(sUser[9]));
                    }
                    else
                    {
                        u = new Admin(int.Parse(sUser[0]), sUser[3], sUser[2], sUser[1], sUser[4], sUser[5], int.Parse(sUser[6]), int.Parse(sUser[7]));
                    }
                    if (u.ValidPassword(password))
                    {
                        SetUserAtual(u);
                        return true;
                    }
                }
            }
            return false;
        } 

        public int GetUserType()
        {
            return user_atual.UserType();
        }

        public Leilao getLeilaoWithId(int leilaoId)
        {
            Leilao l = null;// new Leilao(IDBFacade.GetLeilaoWithId(leilaoId));
            return l;

        }


    }
}
