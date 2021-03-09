using infludash_api.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Attributes.User
{
    public enum EmailUniqueType
    {
        Login,
        Register
    }
    public class EmailUniqueAttribute : ValidationAttribute
    {
        public EmailUniqueType _type { get; set; }
        public EmailUniqueAttribute(EmailUniqueType type)
        {
            this._type = type;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (InfludashContext)validationContext
               .GetService(typeof(InfludashContext));

            if (value is string email)
            {
                switch (_type)
                {
                    case EmailUniqueType.Login:
                        if (_context.users.ToList().Exists(x => x.email.Equals(email)))
                        {
                            return ValidationResult.Success;
                        }
                        return new ValidationResult("Credentials do not match our records.");
                    case EmailUniqueType.Register:
                        if (_context.users.ToList().Exists(x => x.email.Equals(email)))
                        {
                            return new ValidationResult("Email already exists.");
                        }
                        return ValidationResult.Success;
                }
                
            }
            return new ValidationResult("Invalid data.");
        }

    }
}
