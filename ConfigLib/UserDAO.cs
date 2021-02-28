using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigLib
{
    public class UserDAO
    {
        public List<User> SelectTable(Database db)
        {
            db.MakeSQLConnection();
            MySqlConnection myConnection = db.GetSqlConnection();
            string statement = "SELECT * FROM `users`";
            MySqlCommand cmd = new MySqlCommand(statement, myConnection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            List<User> list = new List<User>();
            while (dataReader.Read())
            {
                list.Add(new User(dataReader.GetString("user_name"), dataReader.GetString("user_email"), 
                    dataReader.GetString("user_password"), dataReader.GetDateTime("user_email_verified"), 
                    dataReader.GetDateTime("user_created_at")));
            }
            dataReader.Close();
            db.CloseSqlConnection();
            return list;
        }

        public bool EmailExists(Database db, string email)
        {
            db.MakeSQLConnection();
            MySqlConnection myConnection = db.GetSqlConnection();
            string statement = "SELECT email FROM `users` WHERE email = @email";
            MySqlCommand cmd = new MySqlCommand(statement, myConnection);
            cmd.Parameters.AddWithValue("@email", email);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Close();
            return false;
        }
    }
}
