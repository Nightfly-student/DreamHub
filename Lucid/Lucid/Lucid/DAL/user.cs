using Lucid.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace Lucid
{
    class user
    {
        public class userDAO
        {
            public string username;
            private SqlConnection dbConnection;


            public userDAO(string connectionstring)
            {
                dbConnection = new SqlConnection(connectionstring);
            }

            public List<ReadUser> GetALL()
            {
                LoginPage login = new LoginPage();
                dbConnection.Open();
                SqlCommand command = new SqlCommand("SELECT [user_id] FROM [users] WHERE [user_name]='" + username + "'", dbConnection);
                SqlDataReader reader = command.ExecuteReader();
                List<ReadUser> users = new List<ReadUser>();
                while (reader.Read())
                {
                    ReadUser user = readUser(reader);
                    users.Add(user);
                }
                reader.Close();
                dbConnection.Close();

                return users;
            }

            private ReadUser readUser(SqlDataReader reader)
            {
                int id = (int)reader["user_id"];

                return new ReadUser(id);

            }
        }
    }
}
