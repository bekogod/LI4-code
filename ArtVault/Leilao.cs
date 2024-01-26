using Microsoft.AspNetCore.Http.Timeouts;

namespace ArtVault
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
            this.precoReferencia = 0;
            this.precoReserva = precoReserva;
            this.imagem = imagem;
            this.dimensoes = dimensoes;
            this.descricao = descricao;
            this.tipo = tipo;
        }

        public int GetId() { return id; }
        public int GetIdUtilizador() {  return id_utilizador; }
        public DateTime GetDataCom() { return dataCom;}
        public DateTime GetDataFim() {  return dataFim; }
        public string GetNome() {  return nome; }
        public int GetPrecoReferencia() {  return precoReferencia; }
        public int GetPrecoReserva() {  return precoReserva; }
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
                return (preco >= precoReferencia * 1.1);
            }
        }

    }
}
