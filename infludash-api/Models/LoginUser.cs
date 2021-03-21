using infludash_api.Attributes.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Models
{
    public class LoginUser
    {
        [Required, EmailAddress, EmailUnique(EmailUniqueType.Login)]
        public string email { get; set; }
        [Required, DataType(DataType.Password)]
        public string password { get; set; }
    }
}
