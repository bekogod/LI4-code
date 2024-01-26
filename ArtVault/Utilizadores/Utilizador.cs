namespace ArtVault.Utilizadores
{
    public abstract class Utilizador
    {   
        protected int id;
        protected string email;
        protected string password;
        protected string username;
        protected string nome;
        protected string morada;
        protected string nif;
        protected string cc;

        protected Utilizador(int id, string email, string password, string username, string nome, string morada, string nif, string cc)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.username = username;
            this.nome = nome;
            this.morada = morada;
            this.nif = nif;
            this.cc = cc;
        }

        public int GetId() { return id; }
        public string GetEmail() { return email; }
        public string GetPassword() { return password; }
        public string GetUsername() { return username; }
        public string GetNome() { return nome; }
        public string GetMorada() { return morada; }
        public string GetNif() { return nif; }
        public string GetCc() { return cc; }

    }
}
