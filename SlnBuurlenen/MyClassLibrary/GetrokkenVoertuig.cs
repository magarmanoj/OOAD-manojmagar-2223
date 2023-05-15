using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class GetrokkenVoertuig : Voertuig
    {
        public int? Gewicht { get; set; }
        public int? MaxBelasting { get; set; }
        public string Afmetingen { get; set; }
        public bool Geremd { get; set; }
    }

    //public static GetrokkenVoertuig GetGetrokkenVoertuigen()
    //{
    //    string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

    //    using (SqlConnection connection = new SqlConnection(connString))
    //    {
    //        connection.Open();
    //        string query = "SELECT * FROM Voertuigen";
    //        using (SqlCommand command = new SqlCommand(query, connection))
    //        {
    //            // Execute the query and retrieve the results
    //            using (SqlDataReader reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    string type = reader["Type"].ToString();
    //                    if (type == "gemotoriseerd")
    //                    {
                            
    //                    }
    //                    else if (type == "getrokkenvoertuig")
    //                    {
    //                        // Process as getrokkenvoertuig vehicle
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
