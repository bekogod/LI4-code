using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;
using System.Text;

namespace ArtVault.DAOs
{
    public class LeilaoDAO
    {
        private DAOConfig daoConfig;
        public LeilaoDAO(DAOConfig dConfig)

        {

            daoConfig = dConfig;
        }


        public void Start()
        {


            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {

                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Leilao"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {

                            Console.WriteLine($"Column1: {reader["id"]}");   
                        }


                        reader.Close();
                    }

                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void InsertLeilao(int id_utilizador, DateTime dataCom, DateTime dataFim, string nome, int? precoReferencia, int? precoReserva, string imagem, string dimensoes, string? descricao,int tipoLeilao)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Leilao (id_utilizador, dataCom, dataFim, nome, precoReferencia, precoReserva, imagem, dimensoes, descricao,tipoLeilao)
                                     VALUES (@IdUtilizador, @DataCom, @DataFim, @Nome, @PrecoReferencia, @PrecoReserva, @Imagem, @Dimensoes, @Descricao,@TipoLeilao)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@DataCom", dataCom);
                        command.Parameters.AddWithValue("@DataFim", dataFim);
                        command.Parameters.AddWithValue("@Nome", nome);
                        command.Parameters.AddWithValue("@PrecoReferencia", (object)precoReserva ?? DBNull.Value);
                        command.Parameters.AddWithValue("@PrecoReserva", (object)precoReserva ?? DBNull.Value); // Handle nullability
                        command.Parameters.AddWithValue("@Imagem", imagem);
                        command.Parameters.AddWithValue("@Dimensoes", dimensoes);
                        command.Parameters.AddWithValue("@Descricao", (object)descricao ?? DBNull.Value);
                        command.Parameters.AddWithValue("@TipoLeilao", tipoLeilao);


                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Rows affected: {rowsAffected}");


                    }


                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


        public string GetLeilaoByID(int id)
        {
            string leilaoString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Leilao WHERE id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                leilaoString = $"{reader["id"]};{reader["id_utilizador"]};{reader["datacom"]};{reader["datafim"]};{reader["nome"]};{reader["precoreferencia"]};{reader["precoreserva"]};{reader["imagem"]};{reader["dimensoes"]};{reader["descricao"]};{reader["tipoleilao"]}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    daoConfig.CloseConnection(connection);
                }
            }
            Console.WriteLine(leilaoString);
            return leilaoString;
        }





        public string GetXLeiloes(int x)
        {
            string leiloesString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"
                    SELECT TOP(@X) * FROM Leilao WHERE dataFim > GETDATE()";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@X", x);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            bool primeiroLeilao = true; //verificar se é o primeiro leilão
                            while (reader.Read())
                            {
                                string leilaoString = $"{reader["id"]};{reader["id_utilizador"]};{reader["datacom"]};{reader["datafim"]};{reader["nome"]};{reader["precoreferencia"]};{reader["precoreserva"]};{reader["imagem"]};{reader["dimensoes"]};{reader["descricao"]};{reader["tipoleilao"]}";
                                if (primeiroLeilao)
                                {
                                    leiloesString = leilaoString;
                                    primeiroLeilao = false;
                                }
                                else
                                {
                                    leiloesString += "|" + leilaoString;
                                }
                             
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    daoConfig.CloseConnection(connection);
                }
            }

            return leiloesString;
        }

        public string GetLeilaoByUserID(int id_utilizador)
        {
            string leilaoString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Leilao WHERE id_utilizador = @IdUtilizador";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                leilaoString = $"{reader["id"]};{reader["id_utilizador"]};{reader["datacom"]};{reader["datafim"]};{reader["nome"]};{reader["precoreferencia"]};{reader["precoreserva"]};{reader["imagem"]};{reader["dimensoes"]};{reader["descricao"]};{reader["tipoleilao"]}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    daoConfig.CloseConnection(connection);
                }
            }

            return leilaoString;
        }



        public void UpdatePrecoReferencia(int idLeilao, int novoPrecoReferencia)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"UPDATE Leilao SET precoreferencia = @NovoPrecoReferencia WHERE id = @IdLeilao";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NovoPrecoReferencia", novoPrecoReferencia);
                        command.Parameters.AddWithValue("@IdLeilao", idLeilao);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    daoConfig.CloseConnection(connection);
                }
            }
        }












    }

}