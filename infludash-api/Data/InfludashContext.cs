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
        public DbSet<Post> posts { get; set; }
        public DbSet<YtPreference> yt_preferences { get; set; }
        public InfludashContext(DbContextOptions<InfludashContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Social>()
                .HasKey(e => new { e.socialId, e.email });
        }
    }
}
