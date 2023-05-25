using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MyClassLibrary
{
    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public Enums.OntleningStatus Status { get; set; }
        public Voertuig Voertuig { get; set; }
        public Gebruiker Aanvrager { get; set; }
        public string VoertuigNaam { get; set; }

        public static List<Ontlening> GetOntleningen(int aanvragerId)
        {
            List<Ontlening> ontleningen = new List<Ontlening>();
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT Voertuig.naam, Ontlening.id, Ontlening.vanaf, Ontlening.tot, Ontlening.status FROM Ontlening INNER JOIN Voertuig ON Ontlening.Voertuig_id = Voertuig.Id WHERE Ontlening.Aanvrager_id = @Id", conn);
                command.Parameters.AddWithValue("@Id", aanvragerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ontlening ontl = new Ontlening
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            VoertuigNaam = reader.GetString(reader.GetOrdinal("naam")),
                            Vanaf = reader.GetDateTime(reader.GetOrdinal("vanaf")),
                            Tot = reader.GetDateTime(reader.GetOrdinal("tot")),
                            Status = (Enums.OntleningStatus)reader.GetByte(reader.GetOrdinal("status"))
                        };
                        ontleningen.Add(ontl);
                    }
                }
                return ontleningen;
            }
        }

        public static void RemoveOntlening(int ontleningId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
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
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

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

        public static List<Ontlening> GetAanvraagOntleningen(int aanvragerId)
        {
            List<Ontlening> aanvraagOntleningen = new List<Ontlening>();
            List<Ontlening> ontleningen = GetOntleningen(aanvragerId);

            foreach (Ontlening ontlening in ontleningen)
            {
                if (ontlening.Status == Enums.OntleningStatus.InAanvraag)
                {
                    aanvraagOntleningen.Add(ontlening);
                }
            }

            return aanvraagOntleningen;
        }
    }
}
