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

 
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {

                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Lance";

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
                        command.Parameters.AddWithValue("@IdUtilizador", id_utilizador);
                        command.Parameters.AddWithValue("@IdLeilao", id_leilao);
                        command.Parameters.AddWithValue("@DataHora", dataHora);
                        command.Parameters.AddWithValue("@Valor", valor);

                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine($"Rows affected: {rowsAffected}");

                    }

                    // Close the connection when done
                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }



        }


        public string GetXLancesByLeilaoID(int id_leilao, int x)
        {
            string lancesString ="";

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
            if (lancesString == null) return "";

            else return lancesString;
        }






    }
}