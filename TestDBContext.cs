using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testovoe
{
    public partial class TestDBContext : DbContext
    {
        public TestDBContext()
        { }
        public virtual DbSet<Bagette> Bagettes { get; set; }
        public virtual DbSet<Crousant> Crousants { get; set; }
        public virtual DbSet<Crendel> Crendels { get; set; }
        public virtual DbSet<Smetannik> Smetanniks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=TestDB; Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
