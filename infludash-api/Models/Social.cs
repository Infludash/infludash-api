using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Models
{
    public class Social
    {
        [Key, Required]
        public string accessToken { get; set; }
        [Required]
        public string socialId { get; set; }
        [Required]
        public SocialType type { get; set; }
        [Required]
        public string email { get; set; }
    }
}
