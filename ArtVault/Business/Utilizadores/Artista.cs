﻿namespace ArtVault.Business.Utilizadores
{
    public class Artista : Utilizador
    {
        private bool ativo;

        public Artista(int id, string email, string password, string username, string nome, string morada, int nif, int cc, int ativo)
                : base(id, email, password, username, nome, morada, nif, cc)
        {
            if (ativo == 0)
                this.ativo = false;
            else this.ativo =true;
        }

        public void AtivarConta()
        {
            ativo = true;
        }

        public override int UserType() { return 2; }
    }
}