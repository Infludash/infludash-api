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
        [Required]
        public string accessToken { get; set; }
        [Required, Key]
        public string socialId { get; set; }
        [Required]
        public SocialType type { get; set; }
        [Required, Key]
        public string email { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string imageUrl { get; set; }
        public string regionCode { get; set; }
        public string pageIds { get; set; }
    }
}
