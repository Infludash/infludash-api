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
        [Required, DataType(DataType.Password), PasswordValidation(ErrorMessage = "Password must contain at least 8 characters with at least one special character, a number, an uppercase character and no whitespaces.")]
        public string password { get; set; }
    }
}
