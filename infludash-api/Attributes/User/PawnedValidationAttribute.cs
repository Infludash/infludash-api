using infludash_api.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace infludash_api.Attributes.User
{
    public class PawnedValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string password)
            {
                string sha1Hash = Helper.GetSha1(password);
                string prefix = sha1Hash.Substring(0, 5);
                string suffix = sha1Hash.Substring(5, sha1Hash.Length - prefix.Length);
                string response = Helper.HttpGetRequest($"https://api.pwnedpasswords.com/range/{prefix}");

                string[] lines = response.Split('\n');
                foreach (var line in lines)
                {
                    string[] temp = line.Split(':');
                    string tempSuffix = temp[0];
                    int count = int.Parse(temp[1]);

                    if (suffix == tempSuffix && count >= 300)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
