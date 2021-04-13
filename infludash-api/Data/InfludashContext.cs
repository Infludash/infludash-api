using infludash_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infludash_api.Data
{
    public class InfludashContext : DbContext
    {
        public DbSet<Social> socials { get; set; }
        public InfludashContext(DbContextOptions<InfludashContext> options) : base(options)
        {
        }
    }
}
