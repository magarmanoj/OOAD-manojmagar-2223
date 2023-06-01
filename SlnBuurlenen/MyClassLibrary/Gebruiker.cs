﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

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

        public string Profielfoto { get; set; }

        public Enums.GeslachtType Geslacht { get; set; }

        public static Gebruiker FindByLoginAndPassword(string login, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM [GEBRUIKER] WHERE email = @Email", connection);
                command.Parameters.AddWithValue("@Email", login);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Gebruiker gebruiker = new Gebruiker();
                    gebruiker.Id = (int)reader["Id"];
                    gebruiker.Voornaam = (string)reader["voornaam"];
                    gebruiker.Achternaam = (string)reader["achternaam"];
                    string storedPassword = (string)reader["paswoord"];

                    bool isPasswordCorrect = VerifyPassword(password, storedPassword);
                    if (isPasswordCorrect)
                    {
                        return gebruiker;
                    }
                    else
                    {
                        string hashedStoredPassword = ToSha256(storedPassword);
                        string hashedInputPassword = ToSha256(password);
                        if (hashedInputPassword == hashedStoredPassword)
                        {

                            return gebruiker;
                        }
                    }

                }
                return null;
            }
        }

        private static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string hashedInputPassword = ToSha256(inputPassword);
            return hashedInputPassword == hashedPassword;
        }

        public static Gebruiker GetGebruikerById(int gebruikerId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT * FROM [Gebruiker] WHERE Id = @GebruikerId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GebruikerId", gebruikerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Gebruiker gebruiker = new Gebruiker();
                        gebruiker.Id = (int)reader["Id"];
                        gebruiker.Voornaam = reader.GetString(reader.GetOrdinal("Voornaam"));
                        gebruiker.Achternaam = reader.GetString(reader.GetOrdinal("Achternaam"));
                        return gebruiker;
                    }
                }
            }
            return null; 
        }

        public static string ToSha256(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashedPasswordBytes = sha256.ComputeHash(inputBytes);
                string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "");
                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }
        public static void StoreHashedPassword(string hashedPassword, int userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "UPDATE [Gebruiker] SET paswoord = @HashedPassword Where id = @ID";
                
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                command.Parameters.AddWithValue("@ID", userId);

                command.ExecuteNonQuery();
            }
        }
    }
}
