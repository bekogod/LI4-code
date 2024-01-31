using ArtVault.Business.Utilizadores;
using ArtVault.DAOs;
using System.IO;
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

        public string GetUserAtualId()
        {
            return user_atual.GetId().ToString();
        }

        public void SetLeilaoAtual(Leilao leilao)
        {
            leilao_atual = leilao;
        }


        public async Task<bool> CheckCredentialsAsync(string email, string password)
        {
            string user = await Task.Run(() => IDBFacade.GetUserByEmail(email));
            
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

                        if (!bool.Parse(sUser[9]))
                        {
                            return false;
                        }
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
            bool active = true;
            if (tipo == 2) { active = false; }
            if (valid == 0)
            {
                IDBFacade.InsertUtilizador(username, password, email, nome, morada, nif, cc, tipo, active);
                return true;
            }
            return false;
        }

        public int GetUserType()
        {
            return user_atual.UserType();
        }


        public bool TryLance(int id_leilao, int valor)
        {
            string l = IDBFacade.GetLeilaoByID(id_leilao);
            leilao_atual = new Leilao(l);
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

        static void SalvarImagem(byte[] bytesDaImagem, string caminhoDoArquivo)
        {
            // Use FileStream para criar ou abrir o arquivo no modo de escrita
            using (FileStream fs = new FileStream(caminhoDoArquivo, FileMode.Create))
            {
                // Escreva os bytes da imagem no arquivo
                fs.Write(bytesDaImagem, 0, bytesDaImagem.Length);
            }
        }
        public async Task<byte[]> GetImg(string imagePath)
        {
            try
            {
                // Specify the full path to the image on the server
                string fullImagePath = "./ImgsLeiloes/" + imagePath;

                // Read the image file as a byte array
                byte[] imageData = await File.ReadAllBytesAsync(fullImagePath);

                return imageData;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, access denied) appropriately
                // Log the exception or return a default image, depending on your requirements
                Console.WriteLine($"Error fetching image: {ex.Message}");
                return null;
            }
        }

        public bool TryLeilao(string nome, int tipo, int precoReservado, int? precoInicial, string imageName, string dimensoes, DateTime dataFim, string? descricao, byte[] bImagem)
        {
            IArtVaultFacade.SalvarImagem(bImagem,  "./ImgsLeiloes/" + imageName);
            DateTime dataCom = DateTime.Now;
            int id_leilao = IDBFacade.InsertLeilao(user_atual.GetId(), dataCom, dataFim, nome, precoInicial, precoReservado, imageName, dimensoes, descricao, tipo);
            IDBFacade.InsertWatchlist(user_atual.GetId(), id_leilao);
            return true;
        }

        public async Task AceitarArtistaAsync(int id_artista)
        {
            await Task.Run(() => IDBFacade.ActivateUtilizador(id_artista));
        }

        public async Task RejeitarArtistaAsync(int id_artista)
        {
            await Task.Run(() => IDBFacade.DeleteUtilizador(id_artista));
        }
        public void CancelarLeilao(int id_leilao)
        {   
            //método DB que elimina todas as referências a um leilão das WLs
            IDBFacade.DeleteLeiloesFromWL(id_leilao);

            //método DB eliminar lances de um leilão
            IDBFacade.RemoveLancesFromLeilaoID(id_leilao);
            
            //método DB que elimina esse leilão dado o id
            IDBFacade.DeleteLeilao(id_leilao);
        }

        public Leilao GetLeilaoWithID(int id_leilao)
        {
            string leilao = IDBFacade.GetLeilaoByID(id_leilao);
            Leilao l = new Leilao(leilao);
            return l;
        }

        public string GetNomeArtista(int id_artista)
        {
            return IDBFacade.GetNameByID(id_artista);
            
        }

        public List<Leilao> GetXLeiloes(int x)
        {
            List<Leilao> result = new List<Leilao>();
            string leiloes = IDBFacade.GetBotXLeiloes(x);
            string[] larray = leiloes.Split('|');
            List<int> ids = new List<int>();
            foreach (string l in larray)
            {
                Leilao novo_leilao = new Leilao(l);
                ids.Add(novo_leilao.GetId());
                result.Add(novo_leilao);
            }

            List<int> inWL = IDBFacade.VariosInWL(ids, user_atual.GetId());
            
            foreach (Leilao leilao in result)
            {
                if (inWL.Contains(leilao.GetId()))
                {
                    leilao.SetInWL(true);
                }
                else
                {
                    leilao.SetInWL(false);
                }
            }
            return result;
        }


        public async Task AddLeilaoToWLAsync(int id_leilao)
        {
            await Task.Run(() => IDBFacade.InsertWatchlist(user_atual.GetId(), id_leilao));
        }

        public async Task RemoveFromWLAsync(int id_leilao)
        {
            await Task.Run(() => IDBFacade.RemoveFromWL(user_atual.GetId(), id_leilao));
        }

        public async Task<List<Leilao>> GetLeiloesWLAsync()
        {
            List<Leilao> result = new List<Leilao>();

            string leiloes = await Task.Run(() => IDBFacade.GetAllLeiloesInWLofUtilizadorString(user_atual.GetId()));

            if (leiloes.Length != 0)
            {
                string[] larray = leiloes.Split('|');
                foreach (string l in larray)
                {
                    Leilao novo_leilao = new Leilao(l);
                    novo_leilao.SetInWL(true);
                    result.Add(novo_leilao);
                }
            }

            return result;
        }

        public async Task<List<Lance>> GetXLancesByLeilaoIDAsync(int id_leilao, int x)
        {
            List<Lance> result = new List<Lance>();
            string lances = await Task.Run(() => IDBFacade.GetXLancesByLeilaoID(id_leilao, x));

            if (lances.Length != 0)
            {
                string[] larray = lances.Split('|');

                foreach (string l in larray)
                {
                    Lance novo_lance = new Lance(l);
                    int id_utilizador = novo_lance.GetIdUtilizador();
                    novo_lance.SetUsername(await Task.Run(() => IDBFacade.GetUserNameByID(id_utilizador)));
                    result.Add(novo_lance);
                }
            }
            
            return result;
        }


        public async Task<List<Utilizador>> GetUsersPorValidarAsync()
        {
            List<Utilizador> result = new List<Utilizador>();
            string users = await Task.Run(() => IDBFacade.GetInactiveUsers());

            if (users.Length != 0)
            {
                string[] uarray = users.Split('|');
                foreach (string u in uarray)
                {
                    string[] sUser = u.Split(';');
                    string type = sUser[8];
                    if (type == "2")
                    {
                        Utilizador artista = new Artista(sUser);
                        result.Add(artista);
                    }
                }
            }

            return result;
        }

    }
}
