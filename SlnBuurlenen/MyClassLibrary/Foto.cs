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
    }

    //public static Foto GetDataOfPhoto()
    //{
    //    string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
    //    using (SqlConnection connection = new SqlConnection(connString))
    //    {
    //        connection.Open();

    //        SqlCommand command = new SqlCommand("SELECT [f].[Id] [f].[Data] CASE WHEN [v].[type] = 1 THEN 'getrokken' ELSE 'motor' END AS [voertuig_type] FROM [BuurlenenDB].[dbo].[Foto] [f] INNER JOIN [BuurlenenDB].[dbo].[Voertuig] [v] ON [f].[VoertuigId] = [v].[Id]WHERE[v].[type] = 1", connection);

    //        SqlDataReader reader = command.ExecuteReader();

    //        if (reader.Read())
    //        {
                
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //        return null;
    //    }
    //}
}
