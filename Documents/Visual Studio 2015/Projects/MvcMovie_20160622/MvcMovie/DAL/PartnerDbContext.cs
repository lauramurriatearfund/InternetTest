
using MvcMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MvcMovie.DAL
{
    public class PartnerDbContext : DbContext
    {
        
        public PartnerDbContext() : base("PartnerDbContext")
        {
        }

        public DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}