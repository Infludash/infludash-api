using System;
using System.Collections.Generic;
using System.Text;
using dotenv.net;

namespace ConfigLib
{
    public class EnvVariables
    {
        public static void LoadEnvironmentVariables()
        {
            string production = Environment.GetEnvironmentVariable("PRODUCTION");
            if (string.IsNullOrEmpty(production))
            {
                var success = DotEnv.AutoConfig();
                if (!success)
                {
                    SetDefaultDevEnv();
                }
            }
        }

        public static void SetDefaultDevEnv()
        {
            Environment.SetEnvironmentVariable("MYSQL_USERNAME", "root");
            Environment.SetEnvironmentVariable("MYSQL_PASSWORD", "");
            Environment.SetEnvironmentVariable("MYSQL_HOST", "localhost");
            Environment.SetEnvironmentVariable("MYSQL_PORT", "3306");
            Environment.SetEnvironmentVariable("MYSQL_DB", "sociodash");
        }
    }
}
