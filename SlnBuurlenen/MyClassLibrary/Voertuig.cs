﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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

        public static List<Voertuig> GetGetrokkenOrMotor(bool isGetrokken, int userId)
        {
            List<Voertuig> voertuigs = new List<Voertuig>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM [Voertuig] WHERE type = @Type AND eigenaar_id <> @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Type", isGetrokken ? 2 : 1);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    voertuigs.Add(new Voertuig(reader));
                }
            }
            return voertuigs;
        }

        public static List<Voertuig> GetAllVoertuigNotOwnedByUser(int userId)
        {
            List<Voertuig> voertuigs = new List<Voertuig>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM [Voertuig] WHERE eigenaar_id <> @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    voertuigs.Add(new Voertuig(reader));
                }
            }
            return voertuigs;
        }

        public static List<Voertuig> GetAllVoertuigOwnedByUser(int userId)
        {
            List<Voertuig> voertuigs = new List<Voertuig>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT * FROM [Voertuig] WHERE eigenaar_id = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    voertuigs.Add(new Voertuig(reader));
                }
            }
            return voertuigs;
        }

        public void DeleteVoertuig(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete "Foto" table first
                        string deleteFotoQuery = "DELETE FROM [Foto] WHERE voertuig_id = @Id";
                        SqlCommand deleteFotoCmd = new SqlCommand(deleteFotoQuery, conn, transaction);
                        deleteFotoCmd.Parameters.AddWithValue("@Id", id);
                        deleteFotoCmd.ExecuteNonQuery();

                        // Delete "Ontlening" table
                        string deleteOntleningQuery = "DELETE FROM [Ontlening] WHERE voertuig_id = @Id";
                        SqlCommand deleteOntleningCmd = new SqlCommand(deleteOntleningQuery, conn, transaction);
                        deleteOntleningCmd.Parameters.AddWithValue("@Id", id);
                        deleteOntleningCmd.ExecuteNonQuery();

                        // Delete "Voertuig" table
                        string deleteVoertuigQuery = "DELETE FROM [Voertuig] WHERE id = @Id";
                        SqlCommand deleteVoertuigCmd = new SqlCommand(deleteVoertuigQuery, conn, transaction);
                        deleteVoertuigCmd.Parameters.AddWithValue("@Id", id);
                        deleteVoertuigCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void AddGetrokkenVoertuig(Voertuig voertuig, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string sql = "INSERT INTO Voertuig (naam, beschrijving, bouwjaar, merk, model, gewicht, maxbelasting, afmetingen, geremd, type, eigenaar_id) " +
                     "VALUES (@Naam, @Beschrijving, @Bouwjaar, @Merk, @Model, @Gewicht, @MaxBelasting, @Afmetingen, @Geremd, @Type, @EigenaarId)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Naam", voertuig.Naam);
                    command.Parameters.AddWithValue("@Beschrijving", voertuig.Beschrijving);
                    command.Parameters.AddWithValue("@Bouwjaar", voertuig.Bouwjaar);
                    command.Parameters.AddWithValue("@Merk", voertuig.Merk);
                    command.Parameters.AddWithValue("@Model", voertuig.Model);
                    command.Parameters.AddWithValue("@Gewicht", voertuig.Gewicht);
                    command.Parameters.AddWithValue("@MaxBelasting", voertuig.MaxBelasting);
                    command.Parameters.AddWithValue("@Afmetingen", voertuig.Afmetingen);
                    command.Parameters.AddWithValue("@Geremd", voertuig.Geremd);
                    command.Parameters.AddWithValue("@Type", 2);
                    command.Parameters.AddWithValue("@eigenaarId", userId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddGemotoriseerdVoertuig(Voertuig voertuig, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string sql = "INSERT INTO Voertuig (naam, beschrijving, bouwjaar, merk, model, type, transmissie, brandstof, eigenaar_id) " +
                     "VALUES (@Naam, @Beschrijving, @Bouwjaar, @Merk, @Model, @Type, @Transmissie, @Brandstof, @EigenaarId)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Naam", voertuig.Naam);
                    command.Parameters.AddWithValue("@Beschrijving", voertuig.Beschrijving);
                    command.Parameters.AddWithValue("@Bouwjaar", voertuig.Bouwjaar);
                    command.Parameters.AddWithValue("@Merk", voertuig.Merk);
                    command.Parameters.AddWithValue("@Model", voertuig.Model);
                    command.Parameters.AddWithValue("@Type", 1);
                    command.Parameters.AddWithValue("@Transmissie", (int)voertuig.Transmissie);
                    command.Parameters.AddWithValue("@Brandstof", (int)voertuig.Brandstof);
                    command.Parameters.AddWithValue("@eigenaarId", userId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
