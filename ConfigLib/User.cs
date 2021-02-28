using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigLib
{
    public class User
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTimeOffset verified { get; set; }
        public DateTimeOffset createdAt { get; set; }

        public User(string name, string email, string password, DateTimeOffset verified, DateTimeOffset createdAt)
        {
            this.name = name;
            this.email = email;
            this.password = password;
            this.verified = verified;
            this.createdAt = createdAt;
        }
    }
}
