
using MvcMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

namespace MvcMovie.DAL
{
    public class UserSessionDbContext : DbContext
    {

        public UserSessionDbContext() : base("UserSessionDbContext")
        {
        }

        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public void save(UserSession session)
        {
            return;
            //TODO - throw necessary exceptions
        }

        public static implicit operator UserSessionDbContext(Session v)
        {
            throw new NotImplementedException();
        }
    }
}