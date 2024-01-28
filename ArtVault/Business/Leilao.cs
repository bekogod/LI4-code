using Microsoft.AspNetCore.Http.Timeouts;

namespace ArtVault.Business
{
    public class Leilao
    {
        private int id;
        private int id_utilizador;
        private DateTime dataCom;
        private DateTime dataFim;
        private string nome;
        private int precoReferencia;
        private int precoReserva;
        private string imagem;
        private string dimensoes;
        private string? descricao;
        private int tipo; //1-normal 2-sealed
        // falta adicionar a estrutura que representa o conjunto de lances
        // talvez possa ser só um lance correspondente à melhor oferta

        public Leilao(int id, int id_utilizador, DateTime dataCom, DateTime dataFim, string nome, int precoReserva, string imagem, string dimensoes, string? descricao, int tipo)
        {
            this.id = id;
            this.id_utilizador = id_utilizador;
            this.dataCom = dataCom;
            this.dataFim = dataFim;
            this.nome = nome;
            precoReferencia = 0;
            this.precoReserva = precoReserva;
            this.imagem = imagem;
            this.dimensoes = dimensoes;
            this.descricao = descricao;
            this.tipo = tipo;
        }

        public Leilao(string leilao)
        {
            string[] array = leilao.Split(';');
            if (array.Length == 11)
            {
                id = int.Parse(array[0]);
                id_utilizador = int.Parse(array[1]);
                dataCom = DateTime.Parse(array[2]);
                dataFim = DateTime.Parse(array[3]);
                nome = array[4];
                precoReferencia = int.Parse(array[5]);
                precoReserva = int.Parse(array[6]);
                imagem = array[7];
                dimensoes = array[8];
                descricao = array[9];
                tipo = int.Parse(array[10]);
            }
        }

        public int GetId() { return id; }
        public int GetIdUtilizador() { return id_utilizador; }
        public DateTime GetDataCom() { return dataCom; }
        public DateTime GetDataFim() { return dataFim; }
        public string GetNome() { return nome; }
        public int GetPrecoReferencia() { return precoReferencia; }
        public int GetPrecoReserva() { return precoReserva; }
        public int GetTipo() { return tipo; }

        public string GetImagem() { return imagem; }
        public string GetDimensoes() { return dimensoes; }
        public string GetDescricao()
        {
            if (descricao != null) return descricao;
            else return "";
        }

        public void SetPrecoReferencia(int novoPreco)
        {
            precoReferencia = novoPreco;
        }

        public bool ValorMinimoLance(int preco)
        {
            if (tipo == 2)
            {
                return true;
            }
            else
            {
                return preco >= precoReferencia * 1.1;
            }
        }

        public string GetTempoRestante()
        {
            TimeSpan tempoRestante = dataFim - DateTime.Now;
            return $"{tempoRestante.Days} dias, {tempoRestante.Hours} horas, {tempoRestante.Minutes} minutos, {tempoRestante.Seconds} segundos";
        }

    }
}
