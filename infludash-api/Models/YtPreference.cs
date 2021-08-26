using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Models
{
    public class YtPreference
    {
        [Required, Key]
        public int id { get; set; }

        [Required]
        public string email { get; set; }

        public string description { get; set; }

        public string categories { get; set; }

        public string tags { get; set; }

    }
}
