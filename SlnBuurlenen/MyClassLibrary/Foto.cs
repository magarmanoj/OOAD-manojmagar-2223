using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Foto
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }

        public Voertuig Voertuig { get; set; }

        public Foto() 
        { 
        }
        public Foto(SqlDataReader reader) 
        {
            Id = Convert.ToInt32(reader["id"]);
            Data = (byte[])reader["data"];
        }
        public static Foto GetFotoByVoertuigId(int voertuigId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM[Foto] WHERE voertuig_id = @VoertuigId", conn);
                cmd.Parameters.AddWithValue("@VoertuigId", voertuigId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read()) return null;
                return new Foto(reader);
            }
        }
    }
}
