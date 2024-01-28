using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ArtVault.DAOs
{
    public class UtilizadorDAO
    {
        private DAOConfig daoConfig;
        public UtilizadorDAO(DAOConfig dConfig)

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

                    string query = "SELECT * FROM Utilizador"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query
                        SqlDataReader reader = command.ExecuteReader();

                        // Process the results
                        while (reader.Read())
                        {

                            Console.WriteLine($"Column1: {reader["id"]}, Column2: {reader["username"]}");   
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
        public void InsertUtilizador(string username, string password, string email, string nome, string morada, int NIF, int CC, byte tipoConta, bool ativo)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Utilizador (username, password, email, nome, morada, NIF, CC, tipoConta, ativo)
                                     VALUES (@Username, @Password, @Email, @Nome, @Morada, @NIF, @CC, @TipoConta, @Ativo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Nome", nome);
                        command.Parameters.AddWithValue("@Morada", morada);
                        command.Parameters.AddWithValue("@NIF", NIF);
                        command.Parameters.AddWithValue("@CC", CC);
                        command.Parameters.AddWithValue("@TipoConta", tipoConta);
                        command.Parameters.AddWithValue("@Ativo", ativo);

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

        public string GetUserByEmail(string email)
        {
            string? userString = null;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Utilizador WHERE email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter for email
                        command.Parameters.AddWithValue("@Email", email);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if the reader has rows (user found)
                            if (reader.Read())
                            {
                                // Construct the user string with parameters separated by ";"
                                userString = $"{reader["id"]};{reader["username"]};{reader["password"]};{reader["email"]};{reader["nome"]};{reader["morada"]};{reader["NIF"]};{reader["CC"]};{reader["tipoConta"]};{reader["ativo"]}";
                                Console.WriteLine(userString);
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
                    // Close the connection when done
                    daoConfig.CloseConnection(connection);
                }
            }

            return userString;
        }


        public int ExisteUtilizador(int NIF, int CC, string username, string email)
        {
            int existeUtilizador = 0;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT COUNT(*) FROM Utilizador 
                             WHERE NIF = @NIF AND CC = @CC AND username = @Username AND email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NIF", NIF);
                        command.Parameters.AddWithValue("@CC", CC);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        int count;
                        count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            existeUtilizador = 1;
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
            return existeUtilizador;
        }





        public void ActivateUtilizador(int idUtilizador)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"UPDATE Utilizador SET ativo = 1 WHERE id = @IdUtilizador";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", idUtilizador);
                        connection.Open();
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

        public void DeleteUtilizador(int idUtilizador)
        {
            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"DELETE FROM Utilizador WHERE id = @IdUtilizador";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUtilizador", idUtilizador);
                        connection.Open();
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