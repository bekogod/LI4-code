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
                        u = new Padrao(sUser);
                    }
                    else if (type == "2")
                    {
                        u = new Artista(sUser);
                    }
                    else
                    {
                        u = new Admin(sUser);
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

        public bool TryRegisto(string email, string password, string username, string nome, string morada, int nif, int cc, byte tipo)
        {
            int valid = IDBFacade.ExisteUtilizador(nif, cc, username, email);
            if (valid == 0)
            {
                IDBFacade.InsertUtilizador(username, password, email, nome, morada, nif, cc, tipo, true);
                return true;
            }
            return false;
        }

        public int GetUserType()
        {
            return user_atual.UserType();
        }


        public bool TryLance(int valor)
        {
            if (leilao_atual.ValorMinimoLance(valor)) // se o valor do lance for válido
            {
                DateTime dateTime = DateTime.Now;
                IDBFacade.InsertLance(user_atual.GetId(), leilao_atual.GetId(), dateTime, valor);
                
                //alterar preço referência do leilão se for caso disso
                if (leilao_atual.GetTipo() == 1)
                {
                    IDBFacade.UpdatePrecoReferencia(leilao_atual.GetId(), valor);
                    leilao_atual.SetPrecoReferencia(valor);
                }
                return true;
            };
            return false;
        }

        public bool TryLeilao(string nome, int tipo, int precoInicial, int? precoReservado, string imagem, string dimensoes, DateTime dataFim, string? descricao)
        {   
            DateTime dataCom = DateTime.Now;
            IDBFacade.InsertLeilao(user_atual.GetId(), dataCom, dataFim, nome, precoInicial, precoReservado, imagem, dimensoes, descricao);
            return true;
        }

        public bool ValidarArtista(int id_artista, bool decisao)
        {
            if (decisao)
            {
                IDBFacade.ActivateUtilizador(id_artista);
            }
            else
            {
                IDBFacade.DeleteUtilizador(id_artista);
            }
                return true;
        }

        public void CancelarLeilao()
        {
            int id_leilao = leilao_atual.GetId();
            //método DB que elimina esse leilão dado o id
        }

        public Leilao GetLeilaoWithID(int id_leilao)
        {
            string leilao = IDBFacade.GetLeilaoByID(id_leilao);
            Leilao l = new Leilao(leilao);
            return l;
        }

        public string GetNomeArtista(int id_artista)
        {
            return IDBFacade.GetUserNameByID(id_artista);
            
        }

        public List<Leilao> GetXLeiloes(int x)
        {
            List<Leilao> result = new List<Leilao>();
            string leiloes = IDBFacade.GetXLeiloes(x);
            string[] larray = leiloes.Split('|');
            List<int> ids = new List<int>();
            foreach (string l in larray)
            {
                Leilao novo_leilao = new Leilao(l);
                ids.Add(novo_leilao.GetId());
                novo_leilao.SetInWL(false);
                result.Add(novo_leilao);
            }

            //List<int> inWL = IDBFacade.VariosInWL(ids, user_atual.GetId());
            
            

            return result;
        }


        public void AddLeilaoToWL(int id_leilao)
        {
            IDBFacade.InsertWatchlist(user_atual.GetId(), id_leilao);
        }

        public void RemoveFromWL(int id_leilao)
        {
            IDBFacade.RemoveFromWL(user_atual.GetId(), id_leilao);
        }
    }
}
