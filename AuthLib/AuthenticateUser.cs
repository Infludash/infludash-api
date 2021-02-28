using ConfigLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthLib
{
    public static class AuthenticateUser
    {
        public static void Login(string email, string password)
        {
            // check if email exists in db
            Database db = new Database();
            db.MakeSQLConnection();

            // if email exists -> get passw hash from db

            // else -> fail

            // check with passw hash if password is correct
            db.CloseSqlConnection();
            // if correct -> success

            // else -> fail
        }

        public static void Register(string name, string email, string password)
        {
            // check if email exists in db
            Database db = new Database();
            db.MakeSQLConnection();

            // if email exists -> fail

            // else -> check if password is strong enough

            // if password is strong enough -> save user

            db.CloseSqlConnection();

            // else -> fail
        }
    }
}
