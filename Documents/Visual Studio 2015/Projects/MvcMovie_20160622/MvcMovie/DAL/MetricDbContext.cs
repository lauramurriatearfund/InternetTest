
using MvcMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

namespace MvcMovie.DAL
{
    public class MetricDbContext : DbContext
    {

        public MetricDbContext() : base("MetricDbContext")
        {
        }

        public DbSet<Metric> Metrics { get; set; }
        public DbSet<UserSession> UserSession { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }



        public static implicit operator MetricDbContext(Models.Metric v)
        {
            throw new NotImplementedException();
        }
    }
}