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

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {

                    // Connection is open, you can perform database operations here
                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Leilao"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query
                        SqlDataReader reader = command.ExecuteReader();

                        // Process the results
                        while (reader.Read())
                        {

                            Console.WriteLine($"Column1: {reader["id"]}");   
                        }

                        // Close the reader
                        reader.Close();
                    }

                    // Close the connection when done
                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during connection
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void InsertLeilao(int id_utilizador, DateTime dataCom, DateTime dataFim, string nome, int precoReferencia, int? precoReserva, string imagem, string dimensoes, string descricao)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Leilao (id_utilizador, dataCom, dataFim, nome, precoReferencia, precoReserva, imagem, dimensoes, descricao)
                                     VALUES (@IdUtilizador, @DataCom, @DataFim, @Nome, @PrecoReferencia, @PrecoReserva, @Imagem, @Dimensoes, @Descricao)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@DataCom", dataCom);
                        command.Parameters.AddWithValue("@DataFim", dataFim);
                        command.Parameters.AddWithValue("@Nome", nome);
                        command.Parameters.AddWithValue("@PrecoReferencia", precoReferencia);
                        command.Parameters.AddWithValue("@PrecoReserva", (object)precoReserva ?? DBNull.Value); // Handle nullability
                        command.Parameters.AddWithValue("@Imagem", imagem);
                        command.Parameters.AddWithValue("@Dimensoes", dimensoes);
                        command.Parameters.AddWithValue("@Descricao", descricao);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        // No need to close the reader for an INSERT operation
                    }

                    // Close the connection when done
                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the insert
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }



        public string GetLeilaoByID(int id)
        {
            string? leilaoString = null;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Leilao WHERE id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        connection.Open();

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

        public string GetXLeiloes (int x)
        {
            string? leiloesString = null;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT TOP(@X) * FROM Leilao";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@X", x);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string leilaoString = $"{reader["id"]};{reader["id_utilizador"]};{reader["datacom"]};{reader["datafim"]};{reader["nome"]};{reader["precoreferencia"]};{reader["precoreserva"]};{reader["imagem"]};{reader["dimensoes"]};{reader["descricao"]};{reader["tipoleilao"]}";
                                leiloesString = leilaoString + ";;";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the search
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    daoConfig.CloseConnection(connection);
                }
            }

            return leiloesString;
        }

       
    }
}