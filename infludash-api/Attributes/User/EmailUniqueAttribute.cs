using infludash_api.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Attributes.User
{
    public class EmailUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (InfludashContext)validationContext
               .GetService(typeof(InfludashContext));

            if (value is string email)
            {
                if (_context.users.ToList().Exists(x => x.email.Equals(email)))
                {
                    return new ValidationResult("Email already exists.");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid data.");
        }

    }
}
