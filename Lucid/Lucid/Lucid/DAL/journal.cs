using Lucid.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lucid.DAL
{
    class journal
    {
        public class journalDAO
        {
            private SqlConnection dbConnection;

            public journalDAO(string connectionstring)
            {
                dbConnection = new SqlConnection(connectionstring);
            }
            public List<ReadJournal> GetALL()
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [journals]", dbConnection);
                SqlDataReader reader = command.ExecuteReader();
                List<ReadJournal> journals = new List<ReadJournal>();
                while (reader.Read())
                {
                    ReadJournal journal = readJournal(reader);
                    journals.Add(journal);
                }
                reader.Close();
                dbConnection.Close();

                return journals;
            }
            public List<ReadJournal> GetByID(int userId)
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [journals] WHERE [user_id] = @Id", dbConnection);
                command.Parameters.AddWithValue("@Id", userId);
                SqlDataReader reader = command.ExecuteReader();
                List<ReadJournal> journals = new List<ReadJournal>();
                while (reader.Read())
                {
                    ReadJournal journal = readJournal(reader);
                    journals.Add(journal);
                }
                reader.Close();
                dbConnection.Close();

                return journals;
            }
/*            public ReadJournal GetByID(int userId)
            {
                dbConnection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM [journals] WHERE [user_id] = @Id", dbConnection);
                command.Parameters.AddWithValue("@Id", userId);
                SqlDataReader reader = command.ExecuteReader();
                ReadJournal journal = null;
                while (reader.Read())
                {
                    journal = readJournal(reader);
                }
                reader.Close();
                dbConnection.Close();

                return journal;
            }*/
            private ReadJournal readJournal(SqlDataReader reader)
            {
                int id = (int)reader["journal_id"];
                string name = (string)reader["journal_name"];
                string description = (string)reader["journal_desc"];
                string cat = (string)reader["journal_cat"];
                bool isLucid = (bool)reader["journal_isLucid"];
                DateTime date = (DateTime)reader["journal_date"];
                bool isPublic = (bool)reader["journal_isPublic"];

                return new ReadJournal(id, name, description, cat,isLucid,date,isPublic);

            }
        }
    }
}
