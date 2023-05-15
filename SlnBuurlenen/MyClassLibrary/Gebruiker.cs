using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Gebruiker
    {
        public int Id { get; set; }

        public string Voornaam { get; set; }

        public string Achternaam { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime Aanmaakdatum { get; set; }

        public bool IsAdmin { get; set; }

        public string Status { get; set; }

        public string Profielfoto { get; set; }

        public Enums.GeslachtType Geslacht { get; set; }

        public static Gebruiker FindByLoginAndPassword(string login, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM [GEBRUIKER] WHERE email = @Email AND paswoord = @Password", connection);
                command.Parameters.AddWithValue("@Email", login);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Gebruiker gebruiker = new Gebruiker();
                    gebruiker.Voornaam = (string)reader["voornaam"];
                    gebruiker.Achternaam = (string)reader["achternaam"];
                    return gebruiker;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
