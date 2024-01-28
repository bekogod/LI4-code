using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ArtVault.DAOs
{
    public class WatchListDAO
    {
        private DAOConfig daoConfig;
        public WatchListDAO(DAOConfig dConfig)

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

                    string query = "SELECT * FROM Watchlist"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query
                        SqlDataReader reader = command.ExecuteReader();

                        // Process the results
                        while (reader.Read())
                        {

                            Console.WriteLine($"user: {reader["id_utilizador"]}, leilao: {reader["id_leilao"]}");   
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
        public void InsertWatchlist(int id_utilizador, int id_leilao)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Watchlist (id_utilizador, id_leilao)
                                     VALUES (@IdUtilizador, @IdLeilao)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);

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


        public string GetLeiloesWatchListByUserId(int id_utilizador)
        {
            string? leiloesString = null;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT Leilao.* FROM WatchList 
                             INNER JOIN Leilao ON WatchList.id_leilao = Leilao.id 
                             WHERE WatchList.id_utilizador = @IdUtilizador";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            bool primeiroLeilao = true;
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

    }
}