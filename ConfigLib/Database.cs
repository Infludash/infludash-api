using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigLib
{
    public class Database
    {
        private MySqlConnection sqlConnection { get; set; }
        public Database()
        {
            EnvVariables.LoadEnvironmentVariables();
            this.sqlConnection = new MySqlConnection($"server={Environment.GetEnvironmentVariable("MYSQL_HOST")};port={Environment.GetEnvironmentVariable("MYSQL_PORT")};user={Environment.GetEnvironmentVariable("MYSQL_USERNAME")};password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};database={Environment.GetEnvironmentVariable("MYSQL_DB")}");
        }

		public void MakeSQLConnection()
		{
			Console.WriteLine("Connecting to database...");
			if (this.sqlConnection.State != System.Data.ConnectionState.Open)
			{
				while (true)
				{
					try
					{
						this.sqlConnection.Open();
						Console.WriteLine("Connected to database!");
						break;
					}
					catch (Exception e)
					{
						Console.WriteLine($"Failed connecting to database. Retrying...\n{e}");
					}
				}
			}
		}
		public void CloseSqlConnection()
		{
			try
			{
				Console.WriteLine("Closing connection to database...");
				this.sqlConnection.Close();
				Console.WriteLine("Connection closed!");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public MySqlConnection GetSqlConnection()
		{
			return sqlConnection;
		}

		public void SafeQuery(string type, string table)
        {

        }
	}
}
