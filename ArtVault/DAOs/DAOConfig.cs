namespace ArtVault.DAOs
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class DAOConfig
    {
        private readonly string connectionString;

        public DAOConfig()
        {
            // Build the connection string
            connectionString = $"Data Source=localhost;Initial Catalog=ArtVault;Integrated Security=True";
        }

        public SqlConnection GetConnection()
        {
            try
            {
                // Create and open a new SqlConnection
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                // Handle any exceptions related to database connection
                Console.WriteLine($"Error establishing database connection: {ex.Message}");
                return null;
            }
        }

        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                // Close the SqlConnection
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions related to closing the database connection
                Console.WriteLine($"Error closing database connection: {ex.Message}");
            }
        }
    }
}
