namespace ArtVault
{
    public class Lance
    {
        private int id;
        private int id_utilizador;
        private int id_leilao;
        private DateTime dataHora;
        private int valor;

        public Lance(int id, int id_utilizador, int id_leilao, DateTime dataHora, int valor)
        {
            this.id = id;
            this.id_utilizador = id_utilizador;
            this.id_leilao = id_leilao;
            this.dataHora = dataHora;
            this.valor = valor;
        }

        public int getId() { return id; }
        public int getId_utilizador() {  return id_utilizador; }
        public int getId_leilao() {  return id_leilao; }
        public DateTime getDataHora() {  return dataHora; }
        public int getValor() {  return valor; }   

        // Não diria que set's sejam necessários
        // Depois de ser criado, um lance não é alterado

    }
}
