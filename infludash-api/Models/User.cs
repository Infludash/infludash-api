using infludash_api.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace infludash_api.Models
{
    public class User
    {
        [Key, Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        [Required, DataType(DataType.Password), PasswordValidation(ErrorMessage = "Password must contain at least 8 characters with at least one special character, a number, an uppercase character and no whitespaces")]
        public string password { get; set; }
        [Required, NotMapped, DataType(DataType.Password), Compare(nameof(password))]
        public string passwordConfirmation { get; set; }
        public DateTime? verified { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
    }
}
