
using System;

namespace ArtVault.DAOs
{
    public class IDatabaseFacade
    {


        private UtilizadorDAO utilizadorDAO;
        private LanceDAO lanceDAO;
        private LeilaoDAO leilaoDAO;
        private WatchListDAO watchlistDAO;

        public IDatabaseFacade()
        {
            // Initialize config
            DAOConfig config = new DAOConfig();

            // Initialize DAOs
            utilizadorDAO = new UtilizadorDAO(config);
            lanceDAO = new LanceDAO(config);
            leilaoDAO = new LeilaoDAO(config);
            watchlistDAO = new WatchListDAO(config);
        }



        //LANCE DAO 
        //LANCE DAO
        //LANCE DAO 
        //LANCE DAO
        
        public void InsertLance(int id_utilizador, int id_leilao, DateTime dataHora, int valor)
        {
            lanceDAO.InsertLance(id_utilizador, id_leilao, dataHora, valor);
        }

        public string GetXLancesByLeilaoID(int id_leilao, int x)
        {
            return lanceDAO.GetXLancesByLeilaoID(id_leilao, x);
        }







        //LEILAO DAO
        //LEILAO DAO
        //LEILAO DAO
        //LEILAO DAO


        public void InsertLeilao(int id_utilizador, DateTime dataCom, DateTime dataFim, string nome, int precoReferencia, int? precoReserva, string imagem, string dimensoes, string? descricao)
        {
            leilaoDAO.InsertLeilao(id_utilizador, dataCom, dataFim, nome, precoReferencia, precoReserva, imagem, dimensoes, descricao);
        }

       
        public string GetLeilaoByID(int id)
        {
            return leilaoDAO.GetLeilaoByID(id);
        }

        public string GetXLeiloes(int x)
        {
            return leilaoDAO.GetXLeiloes(x);
        }

        public string GetLeilaoByUserID(int id_utilizador)
        {
            return leilaoDAO.GetLeilaoByUserID(id_utilizador);
        }

        public void UpdatePrecoReferencia(int idLeilao, int novoPrecoReferencia)
        {
            leilaoDAO.UpdatePrecoReferencia(idLeilao, novoPrecoReferencia);
        }

       




        //UTILIZADOR DAO
        //UTILIZADOR DAO
        //UTILIZADOR DAO
        //UTILIZADOR DAO

        public void InsertUtilizador(string username, string password, string email, string nome, string morada, int NIF, int CC, byte tipoConta, bool ativo)
        {
            utilizadorDAO.InsertUtilizador(username, password, email, nome, morada, NIF, CC, tipoConta, ativo);
        }

        public int ExisteUtilizador(int NIF, int CC, string username, string email)
        {
            return utilizadorDAO.ExisteUtilizador(NIF, CC, username, email);
        }

        public string GetUserByEmail(string email)
        {
            return utilizadorDAO.GetUserByEmail(email);
        }


        public string GetUserByID(int id)
        {
            return utilizadorDAO.GetUserByID(id);
        }

        public string GetUserNameByID(int id)
        {
            return utilizadorDAO.GetUserNameByID(id);
        }


        public void ActivateUtilizador(int idUtilizador)
        {
            utilizadorDAO.ActivateUtilizador(idUtilizador);
        }

        public void DeleteUtilizador(int idUtilizador)
        {
            utilizadorDAO.DeleteUtilizador(idUtilizador);
        }








        //WATCHLIST DAO
        //WATCHLIST DAO
        //WATCHLIST DAO
        //WATCHLIST DAO


        public void InsertWatchlist(int id_utilizador, int id_leilao)
        {
            watchlistDAO.InsertWatchlist(id_utilizador, id_leilao);
        }


        public string GetLeiloesWatchListByUserId(int id_utilizador)
        {
            return watchlistDAO.GetLeiloesWatchListByUserId(id_utilizador);
        }


        public void RemoveFromWL(int id_utilizador, int id_leilao)
        {
            watchlistDAO.RemoveFromWL(id_utilizador,id_leilao);
        }


        public List<int> VariosInWL(List<int> id_leilao, int id_utilizador)
        {
            return watchlistDAO.VariosInWL(id_leilao, id_utilizador);
        }


        public List<int> GetLeiloesOfUtilizadorInWatchList(int id_utilizador)
        {
            return watchlistDAO.GetLeiloesOfUtilizadorInWatchList(id_utilizador);
        }







        public string GetAllLeiloesInWLofUtilizadorString(int id_utilizador)
        {
            string? leiloesExtenso = null;

            List<int> idLeilaoList = watchlistDAO.GetLeiloesOfUtilizadorInWatchList(id_utilizador);

            foreach (int idLeilao in idLeilaoList)
            {

                string leilaoExtenso = leilaoDAO.GetLeilaoByID(idLeilao);

                leiloesExtenso += leilaoExtenso + "|";
            }

            if (leiloesExtenso.Length > 0)
            {
                leiloesExtenso = leiloesExtenso.Remove(leiloesExtenso.Length - 1); // Remove the last character
            }

            return leiloesExtenso;

        }









    }
}
        


    