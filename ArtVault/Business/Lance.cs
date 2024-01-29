namespace ArtVault.Business
{
    public class Lance
    {
        private int id;
        private int id_utilizador;
        private int id_leilao;
        private DateTime dataHora;
        private int valor;
        private string username;

        public Lance(int id, int id_utilizador, int id_leilao, DateTime dataHora, int valor)
        {
            this.id = id;
            this.id_utilizador = id_utilizador;
            this.id_leilao = id_leilao;
            this.dataHora = dataHora;
            this.valor = valor;
        }

        public Lance(string lance)
        {
            string[] array = lance.Split(';');
            if (array.Length == 5)
            {
                id = int.Parse(array[0]);
                id_utilizador = int.Parse(array[1]);
                id_leilao = int.Parse(array[2]);
                dataHora = DateTime.Parse(array[3]);
                valor = int.Parse(array[4]);
            }
        }

        public int GetId() { return id; }
        public int GetIdUtilizador() { return id_utilizador; }
        public int GetIdLeilao() { return id_leilao; }
        public DateTime GetDataHora() { return dataHora; }
        public int GetValor() { return valor; }

        public string GetUsername() { return username; }

        public void SetUsername(string username) {  this.username = username; }

    }
}
