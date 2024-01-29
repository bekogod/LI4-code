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

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {


                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Watchlist"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {

                            Console.WriteLine($"user: {reader["id_utilizador"]}, leilao: {reader["id_leilao"]}");   
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

                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);


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


        public string GetLeiloesWatchListByUserId(int id_utilizador)
        {
            string leiloesString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT Leilao.* FROM Watchlist 
                             INNER JOIN Leilao ON Watchlist.id_leilao = Leilao.id 
                             WHERE Watchlist.id_utilizador = @IdUtilizador";

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



        public void RemoveFromWL(int id_utilizador, int id_leilao)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"DELETE FROM Watchlist WHERE id_utilizador = @IdUtilizador AND id_leilao = @IdLeilao";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);
                        Console.WriteLine(command);

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



        public List<int> VariosInWL(List<int> id_leilao, int id_utilizador)
        {
            List<int> result = [];

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    foreach (int id in id_leilao)
                    {
                        string query = @"SELECT COUNT(*) FROM Watchlist WHERE id_utilizador = @IdUtilizador AND id_leilao = @IdLeilao";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                            command.Parameters.AddWithValue("@IdLeilao", id);

                            int count = (int)command.ExecuteScalar();

                            if (count > 0)
                            {
                                result.Add(id);
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


            return result;
        }


        public List<int> GetLeiloesOfUtilizadorInWatchList(int id_utilizador)
        {
            List<int> result = [];
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT id_leilao FROM Watchlist WHERE id_utilizador = @IdUtilizador";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int idLeilao = (int)reader["id_leilao"];
                                result.Add(idLeilao);
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

            return result;
        }









    }
}
