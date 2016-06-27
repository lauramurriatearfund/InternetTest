
using MvcMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

namespace MvcMovie.DAL
{
    public class EvaluationDbContext : DbContext
    {

        public EvaluationDbContext() : base("EvaluationDbContext")
        {
        }

        public DbSet<Session> Evaluations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public void save(Session evaluation)
        {
            return;
            //TODO - throw necessary exceptions
        }

        public static implicit operator EvaluationDbContext(Session v)
        {
            throw new NotImplementedException();
        }
    }
}