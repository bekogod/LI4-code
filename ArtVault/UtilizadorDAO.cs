using System.Data.SqlClient;

namespace ArtVault
{
    public class UtilizadorDAO
    {

       
        public void Start()
        {
            // Connection string format for SQL Server
            string connectionString = "Data Source=localhost;Initial Catalog=ArtVault;Integrated Security=True";

            // Create a SqlConnection using the connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

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
                            // Access data using reader["ColumnName"]
                            Console.WriteLine($"Column1: {reader["id"]}, Column2: {reader["username"]}");   
                        }

                        // Close the reader
                        reader.Close();
                    }

                    // Close the connection when done
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during connection
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}