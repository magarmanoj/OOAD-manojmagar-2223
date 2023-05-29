using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyClassLibrary
{
    public class Ontlening
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public Enums.OntleningStatus Status { get; set; }
        public Voertuig Voertuig { get; set; }
        public Gebruiker Aanvrager { get; set; }

        public static List<Ontlening> GetOntleningen(int aanvragerId)
        {
            List<Ontlening> ontleningen = new List<Ontlening>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT *, Voertuig.naam FROM [Ontlening] JOIN Voertuig ON Ontlening.Voertuig_id = Voertuig.Id WHERE Ontlening.Aanvrager_id = @Id", conn);
                command.Parameters.AddWithValue("@Id", aanvragerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ontlening ontl = new Ontlening
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Vanaf = reader.GetDateTime(reader.GetOrdinal("vanaf")),
                            Tot = reader.GetDateTime(reader.GetOrdinal("tot")),
                            Status = (Enums.OntleningStatus)reader.GetByte(reader.GetOrdinal("status")),
                            Bericht = !reader.IsDBNull(reader.GetOrdinal("bericht")) ? reader.GetString(reader.GetOrdinal("bericht")) : string.Empty
                        };
                        Voertuig voertuig = new Voertuig();
                        voertuig.Naam = reader.GetString(reader.GetOrdinal("naam"));
                        ontl.Voertuig = voertuig;

                        ontleningen.Add(ontl);
                    }
                }
                return ontleningen;
            }
        }

        public static void RemoveOntlening(int ontleningId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM Ontlening WHERE id = @Id", conn);
                command.Parameters.AddWithValue("@Id", ontleningId);

                command.ExecuteNonQuery();
            }
        }

        public static void AddOntlening(Ontlening nieuweOntlening)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "INSERT INTO Ontlening (voertuig_id, vanaf, tot, bericht, status, aanvrager_id) " +
                               "VALUES (@VoertuigId, @Van, @Tot, @Bericht, @Status, @AanvragerID)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@VoertuigId", nieuweOntlening.Id);
                command.Parameters.AddWithValue("@Van", nieuweOntlening.Vanaf);
                command.Parameters.AddWithValue("@Tot", nieuweOntlening.Tot);
                command.Parameters.AddWithValue("@Bericht", nieuweOntlening.Bericht);
                command.Parameters.AddWithValue("@Status", nieuweOntlening.Status);
                command.Parameters.AddWithValue("@AanvragerId", nieuweOntlening.Aanvrager.Id);

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateOntlening(Ontlening ontlening)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Ontlening SET status = @Status WHERE id = @Id", conn);
                command.Parameters.AddWithValue("@Status", (int)ontlening.Status);
                command.Parameters.AddWithValue("@Id", ontlening.Id);

                command.ExecuteNonQuery();
            }
        }

        public static List<Ontlening> GetAanvraagOntleningen(int aanvragerId)
        {
            List<Ontlening> ontleningen = new List<Ontlening>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT *, Voertuig.naam, Gebruiker.Voornaam, Gebruiker.Achternaam FROM [Ontlening] JOIN Voertuig ON Ontlening.Voertuig_id = Voertuig.Id JOIN Gebruiker ON Ontlening.Aanvrager_id = Gebruiker.Id WHERE Ontlening.Aanvrager_id <> @AanvragerId AND Voertuig.Eigenaar_id = @AanvragerId AND Ontlening.status = 1", conn);
                command.Parameters.AddWithValue("@AanvragerId", aanvragerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ontlening ontl = new Ontlening
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Vanaf = reader.GetDateTime(reader.GetOrdinal("vanaf")),
                            Tot = reader.GetDateTime(reader.GetOrdinal("tot")),
                            Status = (Enums.OntleningStatus)reader.GetByte(reader.GetOrdinal("status")),
                            Bericht = !reader.IsDBNull(reader.GetOrdinal("bericht")) ? reader.GetString(reader.GetOrdinal("bericht")) : string.Empty
                        };
                        Voertuig voertuig = new Voertuig();
                        voertuig.Naam = reader.GetString(reader.GetOrdinal("naam"));
                        ontl.Voertuig = voertuig;

                        Gebruiker aanvrager = new Gebruiker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Aanvrager_id")),
                            Voornaam = reader.GetString(reader.GetOrdinal("Voornaam")),
                            Achternaam = reader.GetString(reader.GetOrdinal("Achternaam"))
                        };
                        ontl.Aanvrager = aanvrager;

                        ontleningen.Add(ontl);
                    }
                }
            }
            return ontleningen;
        }
    }
}
