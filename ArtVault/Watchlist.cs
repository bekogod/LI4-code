namespace ArtVault
{
    public class Watchlist
    {
        private HashSet<int> leiloes;

        public Watchlist()
        {
            leiloes = [];
        }
        public HashSet<int> GetLeiloes() {  return leiloes; }

        public bool AddLeilao(int id_leilao)
        {
            return leiloes.Add(id_leilao);
        }
    }
}
