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


            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {

                    Console.WriteLine("Connected to the database.");

                    string query = "SELECT * FROM Utilizador"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        SqlDataReader reader = command.ExecuteReader();


                        while (reader.Read())
                        {

                            Console.WriteLine($"Column1: {reader["id"]}, Column2: {reader["username"]}");   
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
        public int InsertUtilizador(string username, string password, string email, string nome, string morada, int NIF, int CC, byte tipoConta, bool ativo)
        {
            int insertedUserId = -1;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"INSERT INTO Utilizador (username, password, email, nome, morada, NIF, CC, tipoConta, ativo)
                             OUTPUT INSERTED.id
                             VALUES (@Username, @Password, @Email, @Nome, @Morada, @NIF, @CC, @TipoConta, @Ativo)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Nome", nome);
                        command.Parameters.AddWithValue("@Morada", morada);
                        command.Parameters.AddWithValue("@NIF", NIF);
                        command.Parameters.AddWithValue("@CC", CC);
                        command.Parameters.AddWithValue("@TipoConta", tipoConta);
                        command.Parameters.AddWithValue("@Ativo", ativo);

                        object insertedId = command.ExecuteScalar();

                        if (insertedId != null) int.TryParse(insertedId.ToString(), out insertedUserId);  
                    }

                    daoConfig.CloseConnection(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return insertedUserId;
        }





        public string GetUserByEmail(string email)
        {
            string userString ="";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Utilizador WHERE email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                userString = $"{reader["id"]};{reader["username"]};{reader["password"]};{reader["email"]};{reader["nome"]};{reader["morada"]};{reader["NIF"]};{reader["CC"]};{reader["tipoConta"]};{reader["ativo"]}";
                                Console.WriteLine(userString);
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

            return userString;
        }


        public string GetUserByID(int id)
        {
            string userString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Utilizador WHERE id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userString = $"{reader["id"]};{reader["username"]};{reader["password"]};{reader["email"]};{reader["nome"]};{reader["morada"]};{reader["NIF"]};{reader["CC"]};{reader["tipoConta"]};{reader["ativo"]}";
                                Console.WriteLine(userString);
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

            return userString;
        }


        public string GetUserNameByID(int id)
        {
            string nome = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT nome FROM Utilizador WHERE id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nome = reader["nome"].ToString();
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

            return nome;
        }



        public int ExisteUtilizador(int NIF, int CC, string username, string email)
        {
            int existeUtilizador = 0;

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT COUNT(*)
                                    FROM Utilizador
                                    WHERE (NIF = @NIF OR CC = @CC OR username = @Username OR email = @Email)
                                    AND (NIF IS NOT NULL AND CC IS NOT NULL AND username IS NOT NULL AND email IS NOT NULL);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NIF", NIF);
                        command.Parameters.AddWithValue("@CC", CC);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
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




        public string GetInactiveUsers()
        {
            string inactiveUsersString = "";

            using (SqlConnection connection = daoConfig.GetConnection())
            {
                try
                {
                    string query = @"SELECT * FROM Utilizador WHERE ativo = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            bool isFirstUser = true;
                            while (reader.Read())
                            {
                                string userString = $"{reader["id"]};{reader["username"]};{reader["password"]};{reader["email"]};{reader["nome"]};{reader["morada"]};{reader["NIF"]};{reader["CC"]};{reader["tipoConta"]};{reader["ativo"]}";

                                if (!isFirstUser)
                                {
                                    inactiveUsersString += "|";
                                }
                                else
                                {
                                    isFirstUser = false;
                                }

                                inactiveUsersString += userString;
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


            if (inactiveUsersString == null) return "";
            else return inactiveUsersString;
        }



    }
}