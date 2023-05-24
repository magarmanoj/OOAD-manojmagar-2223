using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Ontlening FindById(int voertuigId, int aanvragerId)
        {
            Ontlening ontlening = new Ontlening();
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT vanaf, tot, bericht FROM [Ontlening] WHERE voertuig_id = @voertuigId AND aanvrager_id = @aanvragerId", connection);
                command.Parameters.AddWithValue("@voertuigId", voertuigId);
                command.Parameters.AddWithValue("@aanvragerId", aanvragerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ontlening.Vanaf = reader.GetDateTime(0);
                        ontlening.Tot = reader.GetDateTime(1);
                        ontlening.Bericht = reader.GetString(2);
                    }
                }
            }
            return ontlening;
        }
    }
}
