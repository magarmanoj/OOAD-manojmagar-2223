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

        // returns the list of the photos
        public static List<Foto> GetFotoListByVoertuigId(int voertuigId)
        {
            List<Foto> fotos = new List<Foto>();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM [Foto] WHERE voertuig_id = @VoertuigId", conn);
                cmd.Parameters.AddWithValue("@VoertuigId", voertuigId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Foto foto = new Foto(reader);
                    fotos.Add(foto);
                }
            }

            return fotos;
        }

        // Returns the first object of the list
        public static Foto GetFotoByVoertuigId(int voertuigId)
        {
            List<Foto> fotos = GetFotoListByVoertuigId(voertuigId);
            return fotos.FirstOrDefault();
        }
    }
}
