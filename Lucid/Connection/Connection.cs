using System;
using System.Data.SqlClient;

namespace Connection
{
    public class Connection
    {
        public void connectionDatabase()
        {
            using (SqlConnection connection = new SqlConnection(ConnectDatabase()))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        public string ConnectDatabase()
        {
           string connectionString = "Data Source=den1.mssql7.gear.host;Initial Catalog=lucidapp; User id=lucidapp;password=Za4fDpqxGH_~";

            return connectionString;
        }
    }

}
