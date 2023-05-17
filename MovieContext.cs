using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreVsDapper
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public string DbPath { get; }

        public MovieContext(string dbPath)
        {
            DbPath = dbPath;
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}