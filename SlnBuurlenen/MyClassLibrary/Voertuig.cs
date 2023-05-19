using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Voertuig
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public int? Bouwjaar { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int Type { get; set; }
        
        // getrokken voertuig
        public int? Gewicht { get; set; }
        public int? MaxBelasting { get; set; }
        public string Afmetingen { get; set; }
        public bool? Geremd { get; set; }
        public int EigenaarId { get; set; }

        // motorvoertuig
        public Enums.TransmissieType? Transmissie { get; set; }

        public Enums.BrandstofType? Brandstof { get; set; }

        public Voertuig() 
        { 
        }
        public Voertuig(SqlDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"]);
            Naam = Convert.ToString(reader["naam"]);
            Beschrijving = Convert.ToString(reader["beschrijving"]);
            Bouwjaar = reader["bouwjaar"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["bouwjaar"]);
            Merk = Convert.ToString(reader["merk"]);
            Model = Convert.ToString(reader["model"]);
            Type = Convert.ToInt32(reader["type"]);
            Transmissie = reader["transmissie"] == DBNull.Value ? null : (Enums.TransmissieType?)(int)reader["transmissie"];
            Brandstof = reader["brandstof"] == DBNull.Value ? null : (Enums.BrandstofType?)(int)reader["brandstof"];
            Gewicht = reader["gewicht"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Gewicht"]);
            MaxBelasting = reader["maxbelasting"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["maxbelasting"]);
            Afmetingen = Convert.ToString(reader["afmetingen"]);
            Geremd = reader["geremd"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(reader["geremd"]);
            EigenaarId = Convert.ToInt32(reader["eigenaar_id"]);
        }

        public static List<Voertuig> GetAllVoertuig() 
        {
            List<Voertuig> voertuigs = new List<Voertuig>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM[Voertuig]", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) voertuigs.Add(new Voertuig(reader));
            }
            return voertuigs;
        }

        public static List<Voertuig> GetGetrokkenOrMotor(bool isGetrokken)
        {
            List<Voertuig> voertuigs = new List<Voertuig>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM [Voertuig] WHERE Type = @Type";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Type", isGetrokken ? 2 : 1);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    voertuigs.Add(new Voertuig(reader));
                }
            }
            return voertuigs;
        }
    }
}
