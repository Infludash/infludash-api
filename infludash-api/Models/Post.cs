using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Models
{
    public class Post
    {
        [Required, Key]
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public DateTime scheduled { get; set; }
        [Required]
        public SocialType type { get; set; }
    }
}
