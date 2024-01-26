using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ArtVault.DAOs
{
    public class UtilizadorDAO
    {
        private DAOConfig daoConfig;
        public UtilizadorDAO()

        {
            // Create a DAOConfig instance
           daoConfig = new DAOConfig();
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
    


}
}