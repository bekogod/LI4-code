
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

        public void InsertUtilizador(string username, string password, string email, string nome, string morada, int NIF, int CC, byte tipoConta, bool ativo)
        {
            utilizadorDAO.InsertUtilizador(username, password, email, nome, morada, NIF, CC, tipoConta, ativo);
        }

        public void InsertLance(int id_utilizador, int id_leilao, DateTime dataHora, int valor)
        {
            lanceDAO.InsertLance(id_utilizador, id_leilao, dataHora, valor);
        }

        public void InsertLeilao(int id_utilizador, DateTime dataCom, DateTime dataFim, string nome, int precoReferencia, int? precoReserva, string imagem, string dimensoes, string descricao)
        {
            leilaoDAO.InsertLeilao(id_utilizador, dataCom, dataFim, nome, precoReferencia, precoReserva, imagem, dimensoes, descricao);
        }

        public void InsertWatchlist(int id_utilizador, int id_leilao)
        {
            watchlistDAO.InsertWatchlist(id_utilizador, id_leilao);
        }

        public string GetUserByEmail(string email)
        {
            return utilizadorDAO.GetUserByEmail(email);
        }
    }
}
        


    