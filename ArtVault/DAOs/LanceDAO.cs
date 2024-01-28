using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ArtVault.DAOs
{
    public class LanceDAO
    {
        private DAOConfig daoConfig;
        public LanceDAO(DAOConfig dConfig)

        {

            daoConfig = dConfig;
        }

        public void selectAll()
        {

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {

                    // Connection is open, you can perform database operations here
                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Lance";

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

        public void InsertLance(int id_utilizador, int id_leilao, DateTime dataHora, int valor)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Lance (id_utilizador, id_leilao, dataHora, valor)
                                     VALUES (@IdUtilizador, @IdLeilao, @DataHora, @Valor)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);
                        command.Parameters.AddWithValue("@DataHora", dataHora);
                        command.Parameters.AddWithValue("@Valor", valor);

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


        public string GetXLancesByLeilaoID(int id_leilao, int x)
        {
            string? lancesString = null;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT TOP(@X) * FROM Lance WHERE id_leilao = @IdLeilao ORDER BY dataHora DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);
                        command.Parameters.AddWithValue("@X", x);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            bool primeiroLance = true;
                            while (reader.Read())
                            {
                                string lanceString = $"{reader["id"]};{reader["id_utilizador"]};{reader["id_leilao"]};{reader["dataHora"]};{reader["valor"]}";
                                if (primeiroLance)
                                {
                                    lancesString = lanceString;
                                    primeiroLance = false;
                                }
                                else
                                {
                                    lancesString += "|" + lanceString;
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
            return lancesString;
        }






    }
}