using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        //public void AddPhotos(byte[] imageData, int voertuigID)
        //{
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand("INSERT INTO [Foto] (data, voertuig_id) VALUES (@ImageData, @VoertuigId)", conn);
        //        cmd.Parameters.AddWithValue("@ImageData", imageData);
        //        cmd.Parameters.AddWithValue("@VoertuigId", voertuigID);
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public int AddPhotos(byte[] imageData, int voertuigID)
        {
            int photoId = 0;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [Foto] (data, voertuig_id) OUTPUT INSERTED.id VALUES (@ImageData, @VoertuigId)", conn);
                cmd.Parameters.AddWithValue("@ImageData", imageData);
                cmd.Parameters.AddWithValue("@VoertuigId", voertuigID);

                photoId = (int)cmd.ExecuteScalar();
            }

            return photoId;
        }

        public void UpdatePhoto(byte[] imageData, int fotoId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [Foto] SET data = @ImageData WHERE id = @FotoId", conn);
                cmd.Parameters.AddWithValue("@ImageData", imageData);
                cmd.Parameters.AddWithValue("@FotoId", fotoId);
                cmd.ExecuteNonQuery();
            }
        }

        //public void DeletePhoto(byte[] imageData)
        //{
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
        //    {
        //        string query = "DELETE FROM [foto] WHERE data = @ImageData";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@ImageData", imageData);

        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void DeletePhoto(int photoId)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                connection.Open();

                string query = "DELETE FROM [foto] WHERE id = @PhotoId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhotoId", photoId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
